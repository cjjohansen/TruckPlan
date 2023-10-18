namespace TruckPlanning
{
    public abstract class Person
    {

        public PersonId PersonId { get; init ; } 

        public PersonName Name { get; protected set; }
        public DateOnly Birthday { get; protected set; }
        

        protected Person(PersonId personId, PersonName PersonName,DateOnly BirthDay)
        {
            PersonId= personId;
            Name = PersonName;
            Birthday = BirthDay;
        } 
    }


    public record PersonId(Guid Id);

    public record PersonName(FirstName FirstName, LastName LastName);

    public record FirstName(string Name); //TODO: Add name validation

    public record LastName(string Name); //TODO: Add name validation

}