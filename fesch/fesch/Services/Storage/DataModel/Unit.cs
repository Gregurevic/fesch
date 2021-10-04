using fesch.Services.Storage.CustomAttributes;

namespace fesch.Services.Storage.DataModel
{
    public abstract class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Neptun Neptun { get; set; }
        private Unit() { }
        public Unit(int id, string name, string neptun)
        {
            Id = id;
            Name = name;
            Neptun = new Neptun(neptun);
        }
    }
}
