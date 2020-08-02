using System;

namespace DocumentLibrary.Models
{
  public class IdentificationDocument : IEquatable<IdentificationDocument>
  {
    public enum IdentificationDocumentType
    {
      Passport, BirthCertificate
    }

    public int Id { get; set; }
    public IdentificationDocumentType DocumentType { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }

    public int DocumentNumber { get; set; }
    public string DocumentSerial { get; set; }
    public string DocumentIssuer { get; set; }
    public string DocumentIssuerCode { get; set; }
    public DateTime DocumentDate { get; set; }

    public bool Equals(IdentificationDocument other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return (DocumentType == other.DocumentType) &&(DocumentNumber == other.DocumentNumber) && ((DocumentType == IdentificationDocumentType.BirthCertificate) || (DocumentSerial == other.DocumentSerial));
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals(obj as IdentificationDocument);
    }

    public override int GetHashCode()
    {
      return DocumentNumber + (int)DocumentType << 30;
    }
  }
}
