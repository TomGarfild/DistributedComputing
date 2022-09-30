namespace Lab4_a;

public class User
{
    public User(string fistName, string lastName, string middleName, string phoneNumber)
    {
        FistName = fistName;
        LastName = lastName;
        MiddleName = middleName;
        PhoneNumber = phoneNumber;
    }

    public string FistName { get; }
    public string LastName { get; }
    public string MiddleName { get; }
    public string PhoneNumber { get; }
    public string FullName => $"{FistName} {LastName} {MiddleName}";

    public override string ToString()
    {
        return $"{FullName} - {PhoneNumber}";
    }
}