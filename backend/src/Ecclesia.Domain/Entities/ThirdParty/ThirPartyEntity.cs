
using  Ecclesia.Domain.Common.Enums;
using Ecclesia.Domain.Entities.ThirdPartyBranches;

namespace Ecclesia.Domain.Entities.ThirdParty;

public class ThirdPartyEntity : BaseEntity
{
    // Identificación
    public string? IdentificationNumber { get; private set; }
    public IdentificationType TypeIden { get; private set; }
    public PersonType PersonType { get; private set; }

    // Persona Natural
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public DateOnly? BirthDate { get; private set; }

    // Persona Jurídica
    public string? BusinessName { get; private set; }
    public string? TradeName { get; private set; }

    // Contacto
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Address { get; private set; }
    public string? City { get; private set; }
    public string? Country { get; private set; }

    // Estado
    public bool IsActive { get; private set; }
    public DateTime RegisteredAt { get; private set; }

    // Roles (null = no tiene ese rol)
    public SupplierInfo? Supplier { get; private set; }
    public MemberInfo? Member { get; private set; }
    public DonorInfo? Donor { get; private set; }
    public EmployeeInfo? Employee { get; private set; }
    public PartnerInfo? Partner { get; private set; }
    public CustomerInfo? Customer { get; private set; }

    // Helpers
    public bool IsSupplier => Supplier != null;
    public bool IsMember => Member != null;
    public bool IsDonor => Donor != null;
    public bool IsEmployee => Employee != null;
    public bool IsPartner => Partner != null;
    public bool IsCustomer => Customer != null;

    protected ThirdPartyEntity() { }

    public ThirdPartyEntity(
        string? identificationNumber,
        IdentificationType typeIden,
        PersonType personType,
        string? firstName = null,
        string? lastName = null,
        DateOnly? birthDate = null,
        string? businessName = null,
        string? tradeName = null,
        string? email = null,
        string? phone = null,
        string? address = null,
        string? city = null,
        string? country = null)
    {
        IdentificationNumber = identificationNumber;
        TypeIden = typeIden;
        PersonType = personType;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        BusinessName = businessName;
        TradeName = tradeName;
        Email = email;
        Phone = phone;
        Address = address;
        City = city;
        Country = country;
        IsActive = true;
        RegisteredAt = DateTime.UtcNow;
    }

    // Métodos de dominio para roles
    public void AssignSupplier(SupplierInfo info)
    {
        if (IsSupplier) throw new InvalidOperationException("Ya tiene el rol de proveedor.");
        Supplier = info;
    }

    public void AssignMember(MemberInfo info)
    {
        if (IsMember) throw new InvalidOperationException("Ya tiene el rol de miembro.");
        Member = info;
    }

    public void AssignDonor(DonorInfo info)
    {
        if (IsDonor) throw new InvalidOperationException("Ya tiene el rol de donante.");
        Donor = info;
    }

    public void AssignEmployee(EmployeeInfo info)
    {
        if (IsEmployee) throw new InvalidOperationException("Ya tiene el rol de empleado.");
        Employee = info;
    }

    public void AssignPartner(PartnerInfo info)
    {
        if (IsPartner) throw new InvalidOperationException("Ya tiene el rol de partner.");
        Partner = info;
    }

    public void AssignCustomer(CustomerInfo info)
    {
        if (IsCustomer) throw new InvalidOperationException("Ya tiene el rol de cliente.");
        Customer = info;
    }

    public void Update(
        string? firstName,
        string? lastName,
        string? businessName,
        string? tradeName,
        string? email,
        string? phone,
        string? address,
        string? city,
        string? country)
    {
        FirstName    = firstName    ?? FirstName;
        LastName     = lastName     ?? LastName;
        BusinessName = businessName ?? BusinessName;
        TradeName    = tradeName    ?? TradeName;
        Email        = email        ?? Email;
        Phone        = phone        ?? Phone;
        Address      = address      ?? Address;
        City         = city         ?? City;
        Country      = country      ?? Country;
    }

    // Métodos para remover roles
    public void RemoveSupplier() => Supplier = null;
    public void RemoveMember() => Member = null;
    public void RemoveDonor() => Donor = null;
    public void RemoveEmployee() => Employee = null;
    public void RemovePartner() => Partner = null;
    public void RemoveCustomer() => Customer = null;

    // Métodos de actualización general
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}