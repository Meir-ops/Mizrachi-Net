namespace mizrachi
{
    public class Customers
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
        public int PersonID { get; set; }
        public string? FullName { get; set; }
        public string? PreferredName { get; set; }
        public string? SearchName { get; set; }
        public bool IsPermittedToLogon { get; set; }
        public string? LogonName { get; set; }
        public bool IsExternalLogonProvider { get; set; }
        public string? HashedPassword { get; set; }
        public bool IsSystemUser { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsSalesperson { get; set; }
        public string? UserPreferences { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FaxNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? Photo { get; set; }
        public string? CustomFields { get; set; }
        public string? OtherLanguages { get; set; }
        public int LastEditedBy { get; set; }
        public DateOnly ValidFrom { get; set; }
        public DateOnly ValidTo { get; set; }
    }
}
