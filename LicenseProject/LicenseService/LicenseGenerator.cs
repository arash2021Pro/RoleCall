using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace LicenseProject.LicenseService
{
    public static class LicenseGenerator
    {

        private const string PrivateKey = @"<RSAKeyValue>
                    <Modulus>
                        mBNKFIc/LkMfaXvLlB/+6EujPkx3vBOvLu8jdESDSQLisT8K96RaDMD1ORmdw2XNdMw/6ZBuJjLhoY13qCU9t7biuL3SIxr858oJ1RLM4PKhA/wVDcYnJXmAUuOyxP/vfvb798o6zAC1R2QWuzG+yJQR7bFmbKH0tXF/NOcSgbc=
                    </Modulus>
                    <Exponent>
                        AQAB
                    </Exponent>
                    <P>
                        xwPKN77EcolMTD2O2Csv6k9Y4aen8UBVYjeQ4PtrNGz0Zx6I1MxLEFzRpiKC/Ney3xKg0Icwj0ebAQ04d5+HAQ==
                    </P>
                    <Q>
                        w568t0Xe6OBUfCyAuo7tTv4eLgczHntVLpjjcxdUksdVw7NJtlnOLApJVJ+U6/85Z7Ji+eVhuN91yn04pQkAtw==
                    </Q>
                    <DP>
                        svkEjRdA4WP5uoKNiHdmMshCvUQh8wKRBq/D2aAgq9fj/yxlj0FdrAxc+ZQFyk5MbPH6ry00jVWu3sY95s4PAQ==
                    </DP>
                    <DQ>
                        WcRsIUYk5oSbAGiDohiYeZlPTBvtr101V669IUFhhAGJL8cEWnOXksodoIGimzGBrD5GARrr3yRcL1GLPuCEvQ==
                    </DQ>
                    <InverseQ>
                        wIbuKBZSCioG6MHdT1jxlv6U1+Y3TX9sHED9PqGzWWpVGA+xFJmQUxoFf/SvHzwbBlXnG0DLqUvxEv+BkEid2w==
                    </InverseQ>
                    <D>                        Yk21yWdT1BfXqlw30NyN7qNWNuM/Uvh2eaRkCrhvFTckSucxs7st6qig2/RPIwwfr6yIc/bE/TRO3huQicTpC2W3aXsBI9822OOX4BdWCec2txXpSkbZW24moXu+OSHfAdYoOEN6ocR7tAGykIqENshRO7HvONJsOE5+1kF2GAE=
                    </D>
                  </RSAKeyValue>";
        
        public static string CreateLicense(XmlLicense licenseData)
        {
            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(PrivateKey);
                var xmlDocument = createXmlDocument(licenseData);
                var xmlDigitalSignature = getXmlDigitalSignature(xmlDocument, provider);
                appendDigitalSignature(xmlDocument, xmlDigitalSignature);
                return xmlDocumentToString(xmlDocument);
            }
        }

        public static string CreateRSAKeyPair(int dwKeySize = 1024)
        {
            using (var provider = new RSACryptoServiceProvider(dwKeySize))
            {
                return provider.ToXmlString(includePrivateParameters: true);
            }
        }

        public static XmlLicense ReadLicense(string licensePublicKey, string xmlFileContent)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xmlFileContent);

                using (var provider = new RSACryptoServiceProvider())
                {
                    provider.FromXmlString(licensePublicKey);

                    var nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");

                    var xml = new SignedXml(doc);
                    var signatureNode = (XmlElement)doc.SelectSingleNode("//sig:Signature", nsmgr);
                    if (signatureNode == null)
                        throw new InvalidOperationException("This license file is not signed.");

                    xml.LoadXml(signatureNode);
                    if (!xml.CheckSignature(provider))
                        throw new InvalidOperationException("This license file is not valid.");

                    var ourXml = xml.GetXml();
                    if (ourXml.OwnerDocument == null || ourXml.OwnerDocument.DocumentElement == null)
                        throw new InvalidOperationException("This license file is coruppted.");

                    using (var reader = new XmlNodeReader(ourXml.OwnerDocument.DocumentElement))
                    {
                        var xmlSerializer = new XmlSerializer(typeof(XmlLicense));
                        return (XmlLicense)xmlSerializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static void appendDigitalSignature(XmlDocument xmlDocument, XmlNode xmlDigitalSignature)
        {
            xmlDocument.DocumentElement.AppendChild(xmlDocument.ImportNode(xmlDigitalSignature, true));
        }

        private static XmlDocument createXmlDocument(XmlLicense licenseData)
        {
            var serializer = new XmlSerializer(licenseData.GetType());
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                var ns = new XmlSerializerNamespaces(); ns.Add("", "");
                serializer.Serialize(writer, licenseData, ns);
                var doc = new XmlDocument();
                doc.LoadXml(sb.ToString());
                return doc;
            }
        }

        private static XmlElement getXmlDigitalSignature(XmlDocument xmlDocument, AsymmetricAlgorithm key)
        {
            var xml = new SignedXml(xmlDocument) { SigningKey = key };
            var reference = new Reference { Uri = "" };
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            xml.AddReference(reference);
            xml.ComputeSignature();
            return xml.GetXml();
        }

        private static string xmlDocumentToString(XmlDocument xmlDocument)
        {
            using (var ms = new MemoryStream())
            {
                var settings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };
                var xmlWriter = XmlWriter.Create(ms, settings);
                xmlDocument.Save(xmlWriter);
                ms.Position = 0;
                return new StreamReader(ms).ReadToEnd();
            }
        }
    }

}