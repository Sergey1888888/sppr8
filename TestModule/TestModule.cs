using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;
using OSMLSGlobalLibrary;
using OSMLSGlobalLibrary.Map;
using OSMLSGlobalLibrary.Modules;

namespace TestModule
{
    public class TestModule : OSMLSModule
    {
        // Различные вирусы
        HumanVirus ospa = new HumanVirus(new Coordinate(4956032, 6225636), "Оспа", "Airborne", 20);
        AnimalVirus animVirus = new AnimalVirus(new Coordinate(4955900, 6225855), "Вирус животного", "Airborne", 20);
        UniversalVirus univ = new UniversalVirus(new Coordinate(4955500, 6226200), "Универсальный вирус", "Contact", 20);
        bool checkGenOspa = true;
        bool checkGenAnim = true;
        bool checkGenUniv = true;
        bool allGood = false;
        protected override void Initialize()
        {
            //Добавление созданных объектов в общий список, доступный всем модулям. Объекты из данного списка отображаются на карте. 
            //тут добавить Human1, Human2, Human3, Human4, Animal1, Animal2, Animal3, Animal4 с разными хар-ами. Все они объекты класса VitalFunctions
            MapObjects.Add(new VitalFunctions(new Coordinate(4956032, 6225636), 1, 100, false, 43, "Human", new Coordinate(4956332, 6225936)));
            MapObjects.Add(new VitalFunctions(new Coordinate(4956030, 6225750), 1, 98, false, 50, "Human", new Coordinate(4956332, 6225936)));
            MapObjects.Add(new VitalFunctions(new Coordinate(4956600, 6225750), 1, 95, false, 50, "Human", new Coordinate(4956332, 6225936)));
            MapObjects.Add(new VitalFunctions(new Coordinate(4955900, 6225855), 1, 97, false, 43, "Animal", new Coordinate(4955800, 6226000)));
            MapObjects.Add(new VitalFunctions(new Coordinate(4955930, 6225845), 1, 90, false, 50, "Animal", new Coordinate(4955800, 6226000)));
            MapObjects.Add(new VitalFunctions(new Coordinate(4956000, 6226000), 1, 99, false, 50, "Animal", new Coordinate(4955500, 6225750)));
            MapObjects.Add(new VitalFunctions(new Coordinate(4955500, 6226200), 1, 89, false, 40, "Human", new Coordinate(4955800, 6225950)));
            MapObjects.Add(new VitalFunctions(new Coordinate(4955901, 6225901), 1, 91, false, 50, "Human", new Coordinate(4956332, 6225936)));
        }
        public override void Update(long elapsedMilliseconds)
        {
            for (int i = 0; i < MapObjects.GetAll<VitalFunctions>().Count; i++)
            {
                if (ospa.generate(MapObjects.GetAll<VitalFunctions>()[i]) == -1 && checkGenOspa)
                {
                    MapObjects.Add(new HumanVirus(new Coordinate(MapObjects.GetAll<VitalFunctions>()[i].X + 1, MapObjects.GetAll<VitalFunctions>()[i].Y + 1), "Оспа", "Airborne", 20, MapObjects.GetAll<VitalFunctions>()[i]));
                    Console.WriteLine("Вирус оспы появился на карте!");
                    checkGenOspa = false;
                    break;
                }
            }
            for (int i = 0; i < MapObjects.GetAll<VitalFunctions>().Count; i++)
            {
                if (animVirus.generate(MapObjects.GetAll<VitalFunctions>()[i]) == -1 && checkGenAnim)
                {
                    MapObjects.Add(new AnimalVirus(new Coordinate(MapObjects.GetAll<VitalFunctions>()[i].X + 1, MapObjects.GetAll<VitalFunctions>()[i].Y + 1), "Вирус животного", "Airborne", 20, MapObjects.GetAll<VitalFunctions>()[i]));
                    Console.WriteLine("Вирус животного появился на карте!");
                    checkGenAnim = false;
                    break;
                }
            }
            for (int i = 0; i < MapObjects.GetAll<VitalFunctions>().Count; i++)
            {
                if (univ.generate(MapObjects.GetAll<VitalFunctions>()[i]) == -1 && checkGenUniv)
                {
                    MapObjects.Add(new UniversalVirus(new Coordinate(MapObjects.GetAll<VitalFunctions>()[i].X + 1, MapObjects.GetAll<VitalFunctions>()[i].Y + 1), "Универсальный вирус", "Contact", 20, MapObjects.GetAll<VitalFunctions>()[i]));
                    Console.WriteLine("Универсальный вирус появился на карте!" + MapObjects.GetAll<UniversalVirus>()[0]);
                    checkGenUniv = false;
                    break;
                }
            }

            if (checkGenOspa == false && checkGenAnim == false && checkGenUniv == false)
            {
                //пытаться заразить всех оспой
                foreach (var vir in MapObjects.GetAll<HumanVirus>())
                {
                    foreach (var person in MapObjects.GetAll<VitalFunctions>())
                    {
                        if (vir.name == "Оспа")
                        {
                            if (vir.infect(person) == 2)
                            {
                                MapObjects.Add(new HumanVirus(new Coordinate(person.X + 1, person.Y + 1), "Оспа", "Airborne", 20, person));
                                Console.WriteLine("Вирус оспы добавлен на карту!");
                            }
                        }
                    }
                }
                //пытаться заразить всех животным вирусом
                foreach (var vir in MapObjects.GetAll<AnimalVirus>())
                {
                    foreach (var person in MapObjects.GetAll<VitalFunctions>())
                    {
                        if (vir.name == "Вирус животного")
                        {
                            if (vir.infect(person) == 2)
                            {
                                MapObjects.Add(new AnimalVirus(new Coordinate(person.X + 1, person.Y + 1), "Вирус животного", "Airborne", 20, person));
                                Console.WriteLine("Вирус животного добавлен на карту!");
                            }
                        }
                    }
                }
                //пытаться заразить всех универсальным вирусом
                foreach (var vir in MapObjects.GetAll<UniversalVirus>())
                {
                    foreach (var person in MapObjects.GetAll<VitalFunctions>())
                    {
                        if (vir.name == "Универсальный вирус")
                        {
                            if (vir.infect(person) == 2)
                            {
                                MapObjects.Add(new UniversalVirus(new Coordinate(person.X + 1, person.Y + 1), "Универсальный вирус", "Contact", 20, person));
                                Console.WriteLine("Универсальный вирус добавлен на карту!");
                            }
                        }
                    }
                }

                foreach (var person in MapObjects.GetAll<VitalFunctions>())
                {
                    if (person.infectionName == "Оспа")
                    {
                        Console.WriteLine("Количество человеческих вирусов на карте: " + MapObjects.GetAll<HumanVirus>().Count);
                        if (person.convalescence(ospa, animVirus, univ, 1) == -1)
                        {
                            Console.WriteLine(person + " вылечился");
                            foreach (var vir in MapObjects.GetAll<HumanVirus>())
                            {
                                if (vir.hum == person)
                                {
                                    Console.WriteLine("Вирус оспы удален с карты!");
                                    MapObjects.Remove(vir);
                                }
                            }
                        }
                    }
                    else if (person.infectionName == "Вирус животного")
                    {
                        Console.WriteLine("Количество животных вирусов на карте: " + MapObjects.GetAll<AnimalVirus>().Count);
                        if (person.convalescence(ospa, animVirus, univ, 2) == -1)
                        {
                            Console.WriteLine(person + " вылечился");
                            foreach (var vir in MapObjects.GetAll<AnimalVirus>())
                            {
                                if (vir.animal == person)
                                {
                                    Console.WriteLine("Вирус животного удален с карты!");
                                    MapObjects.Remove(vir);
                                }
                            }
                        }
                    }
                    else if (person.infectionName == "Универсальный вирус")
                    {
                        Console.WriteLine("Количество универсальных вирусов на карте: " + MapObjects.GetAll<UniversalVirus>().Count);
                        if (person.convalescence(ospa, animVirus, univ, 3) == -1)
                        {
                            Console.WriteLine(person + " вылечился");
                            foreach (var vir in MapObjects.GetAll<UniversalVirus>())
                            {
                                if (vir.who == person)
                                {
                                    Console.WriteLine("Универсальный вирус удален с карты!");
                                    MapObjects.Remove(vir);
                                }
                            }
                        }
                    }

                    int go = person.Move();
                    //идём в рандомное место
                    if (go == -1)
                    {
                        switch (new Random().Next(0, 4))
                        {
                            case 0:
                                person.CoordTo = new Coordinate(person.Coord.X + new Random().Next(0, 500), person.Coord.Y + new Random().Next(0, 500));
                                break;
                            case 1:
                                person.CoordTo = new Coordinate(person.Coord.X + new Random().Next(0, 500), person.Coord.Y - new Random().Next(0, 500));
                                break;
                            case 2:
                                person.CoordTo = new Coordinate(person.Coord.X - new Random().Next(0, 500), person.Coord.Y + new Random().Next(0, 500));
                                break;
                            case 3:
                                person.CoordTo = new Coordinate(person.Coord.X - new Random().Next(0, 500), person.Coord.Y - new Random().Next(0, 500));
                                break;
                        }
                    }
                }

                foreach (var vir in MapObjects.GetAll<HumanVirus>())
                {
                    vir.changeCoord();
                }
                foreach (var vir in MapObjects.GetAll<AnimalVirus>())
                {
                    vir.changeCoord();
                }
                foreach (var vir in MapObjects.GetAll<UniversalVirus>())
                {
                    vir.changeCoord();
                }
                foreach (var person in MapObjects.GetAll<VitalFunctions>())
                {
                    if (person.infected)
                        Console.WriteLine(person + " болен " + person.infectionName + " и имеет " + person.health + " здоровья");
                }
            }
            foreach (var person in MapObjects.GetAll<VitalFunctions>())
            {
                if (person.infected)
                {
                    allGood = false;
                }
                if (allGood)
                {
                    Console.WriteLine("Все вылечились!");
                }
            }
        }
    }

    // Вирус, который воздействует только на организм человека
    [CustomStyle(
        @"new ol.style.Style({ 
            image: new ol.style.Circle({ 
                opacity: 1.0, 
                scale: 1.5, 
                radius: 5, 
                fill: new ol.style.Fill({ 
                    color: 'rgba(255, 0, 0, 0.6)' 
                }), 
                stroke: new ol.style.Stroke({ 
                    color: 'rgb(0, 0, 0)', 
                    width: 1 
                }), 
            }) 
        }); 
        ")]
    public class HumanVirus : Point // Унаследуем данный данный класс от стандартной точки. 
    {
        // Координаты вируса
        public Coordinate humanvirusCoord { get; set; }
        // Название вируса
        public string name { get; set; }
        // Тип инфекции
        public string infectionType { get; set; }
        // Смертельность
        public int mortality { get; set; }
        // Человек с данным вирусом
        public VitalFunctions hum { get; set; }
        // Конструктор для создания нового объекта. 
        public HumanVirus(Coordinate coordinate, string nameVirus, string infectionTypeVirus, int mortalityVirus, VitalFunctions huma = null) : base(coordinate)
        {
            humanvirusCoord = coordinate;
            name = nameVirus;
            infectionType = infectionTypeVirus;
            mortality = mortalityVirus;
            hum = huma;
        }
        // Заражает человека в зависимости от типа заражения
        public int infect(VitalFunctions human)
        {
            if (human.objectType == "Human" && human.infected == false)
            {
                switch (infectionType)
                {
                    case "Airborne":
                        if ((Math.Sqrt(Math.Pow(human.Coord.X - humanvirusCoord.X, 2) + Math.Pow(human.Coord.Y - humanvirusCoord.Y, 2))) < 50)
                        {
                            human.infected = true;
                            human.infectionName = name;
                            human.health = human.health - mortality;
                            human.speed = human.speed * 0.01 * mortality;
                            human.immunity = human.immunity - (0.2 * mortality);
                            hum = human;
                            Console.WriteLine(name + " заразил по воздуху");
                            return 2;
                        }
                        else
                        {
                            Console.WriteLine("Зараженный с " + name + " слишком далеко и не заразил окружающих в 50 метрах.");
                            return 1;
                        }
                    case "Contact":
                        if ((Math.Sqrt(Math.Pow(human.Coord.X - humanvirusCoord.X, 2) + Math.Pow(human.Coord.Y - humanvirusCoord.Y, 2))) < 10)
                        {
                            human.infected = true;
                            human.infectionName = name;
                            human.health = human.health - mortality;
                            human.speed = human.speed * 0.1 * mortality;
                            human.immunity = human.immunity - (0.2 * mortality);
                            hum = human;
                            Console.WriteLine(name + " заразил контактно");
                            return 2;
                        }
                        else
                        {
                            Console.WriteLine("Зараженный с " + name + " слишком далеко и не заразил контактно никого.");
                            return 1;
                        }
                }
            }
            return 1;
        }
        public int generate(VitalFunctions human)
        {
            if (human.objectType == "Human" && human.immunity < 50 && human.infected == false)
            {
                human.infected = true;
                human.infectionName = name;
                human.health = human.health - mortality;
                human.speed = human.speed * 0.01 * mortality;
                human.immunity = human.immunity - (0.2 * mortality);
                hum = human;
                return -1;
            }
            return 1;
        }
        // Изменение координат на координаты больного
        public void changeCoord()
        {
            humanvirusCoord.X = hum.Coord.X;
            humanvirusCoord.Y = hum.Coord.Y;
            X = humanvirusCoord.X + 1;
            Y = humanvirusCoord.Y + 1;
        }
    }

    [CustomStyle(
    @"new ol.style.Style({ 
            image: new ol.style.Circle({ 
                opacity: 1.0, 
                scale: 1.5, 
                radius: 5, 
                fill: new ol.style.Fill({ 
                    color: 'rgba(0, 0, 0, 0.6)' 
                }), 
                stroke: new ol.style.Stroke({ 
                    color: 'rgb(0, 0, 0)', 
                    width: 1 
                }), 
            }) 
        }); 
        ")]
    public class AnimalVirus : Point // Унаследуем данный данный класс от стандартной точки. 
    {
        // Координаты вируса
        public Coordinate animalvirusCoord { get; set; }
        // Название вируса
        public string name { get; set; }
        // Тип инфекции
        public string infectionType { get; set; }
        // Смертельность
        public int mortality { get; set; }
        // Животное с данным вирусом
        public VitalFunctions animal { get; set; }
        // Конструктор для создания нового объекта. 
        public AnimalVirus(Coordinate coordinate, string nameVirus, string infectionTypeVirus, int mortalityVirus, VitalFunctions anim = null) : base(coordinate)
        {
            animalvirusCoord = coordinate;
            name = nameVirus;
            infectionType = infectionTypeVirus;
            mortality = mortalityVirus;
            animal = anim;
        }
        // Заражает животное в зависимости от типа заражения
        public int infect(VitalFunctions animal)
        {
            if (animal.objectType == "Animal" && animal.infected == false)
            {
                switch (infectionType)
                {
                    case "Airborne":
                        if ((Math.Sqrt(Math.Pow(animal.Coord.X - animalvirusCoord.X, 2) + Math.Pow(animal.Coord.Y - animalvirusCoord.Y, 2))) < 50)
                        {
                            animal.infected = true;
                            animal.infectionName = name;
                            animal.health = animal.health - mortality;
                            animal.speed = animal.speed * 0.1 * mortality;
                            animal.immunity = animal.immunity - (0.2 * mortality);
                            Console.WriteLine(name + " заразил по воздуху");
                            return 2;
                        }
                        else
                        {
                            Console.WriteLine("Зараженный с " + name + " слишком далеко и не заразил окружающих в 50 метрах.");
                            return 1;
                        } 
                    case "Contact":
                        if ((Math.Sqrt(Math.Pow(animal.Coord.X - animalvirusCoord.X, 2) + Math.Pow(animal.Coord.Y - animalvirusCoord.Y, 2))) < 10)
                        {
                            animal.infected = true;
                            animal.infectionName = name;
                            animal.health = animal.health - mortality;
                            animal.speed = animal.speed * 0.1 * mortality;
                            animal.immunity = animal.immunity - (0.2 * mortality);
                            Console.WriteLine(name + " заразил контактно");
                            return 2;
                        }
                        else
                        {
                            Console.WriteLine("Зараженный с " + name + " слишком далеко и не заразил контактно никого.");
                            return 1;
                        }
                }
            }
            return 1;
        }
        public int generate(VitalFunctions animal)
        {
            if (animal.objectType == "Animal" && animal.immunity < 50 && animal.infected == false)
            {
                animal.infected = true;
                animal.infectionName = name;
                animal.health = animal.health - mortality;
                animal.speed = animal.speed * 0.1 * mortality;
                animal.immunity = animal.immunity - (0.2 * mortality);
                return -1;
            }
            return 1;
        }
        // Изменение координат на координаты больного
        public void changeCoord()
        {
            animalvirusCoord.X = animal.Coord.X;
            animalvirusCoord.Y = animal.Coord.Y;
            X = animalvirusCoord.X + 1;
            Y = animalvirusCoord.Y + 1;
        }
    }

    [CustomStyle(
@"new ol.style.Style({ 
            image: new ol.style.Circle({ 
                opacity: 1.0, 
                scale: 1.5, 
                radius: 5, 
                fill: new ol.style.Fill({ 
                    color: 'rgba(0, 0, 255, 0.6)' 
                }), 
                stroke: new ol.style.Stroke({ 
                    color: 'rgb(0, 0, 0)', 
                    width: 1 
                }), 
            }) 
        }); 
        ")]
    public class UniversalVirus : Point // Унаследуем данный данный класс от стандартной точки. 
    {
        // Координаты вируса
        public Coordinate universalvirusCoord { get; set; }
        // Название вируса
        public string name { get; set; }
        // Тип инфекции
        public string infectionType { get; set; }
        // Смертельность
        public int mortality { get; set; }
        // Носитель этого вируса
        public VitalFunctions who { get; set; }
        // Конструктор для создания нового объекта. 
        public UniversalVirus(Coordinate coordinate, string nameVirus, string infectionTypeVirus, int mortalityVirus, VitalFunctions whoever = null) : base(coordinate)
        {
            universalvirusCoord = coordinate;
            name = nameVirus;
            infectionType = infectionTypeVirus;
            mortality = mortalityVirus;
            who = whoever;
        }
        // Заражает любого в зависимости от типа заражения
        public int infect(VitalFunctions whoever)
        {
            if (whoever.infected == false)
            {
                switch (infectionType)
                {
                    case "Airborne":
                        if ((Math.Sqrt(Math.Pow(whoever.Coord.X - universalvirusCoord.X, 2) + Math.Pow(whoever.Coord.Y - universalvirusCoord.Y, 2))) < 50)
                        {
                            whoever.infected = true;
                            whoever.infectionName = name;
                            whoever.health = whoever.health - mortality;
                            whoever.speed = whoever.speed * 0.1 * mortality;
                            whoever.immunity = whoever.immunity - (0.2 * mortality);
                            Console.WriteLine(name + " заразил по воздуху");
                            return 2;
                        }
                        else
                        {
                            Console.WriteLine("Зараженный с " + name + " слишком далеко и не заразил окружающих в 50 метрах.");
                            return 1;
                        }
                    case "Contact":
                        if ((Math.Sqrt(Math.Pow(whoever.Coord.X - universalvirusCoord.X, 2) + Math.Pow(whoever.Coord.Y - universalvirusCoord.Y, 2))) < 30)
                        {
                            whoever.infected = true;
                            whoever.infectionName = name;
                            whoever.health = whoever.health - mortality;
                            whoever.speed = whoever.speed * 0.1 * mortality;
                            whoever.immunity = whoever.immunity - (0.2 * mortality);
                            Console.WriteLine(name + " заразил контактно");
                            return 2;
                        }
                        else
                        {
                            Console.WriteLine("Зараженный с " + name + " слишком далеко и не заразил контактно никого.");
                            return 1;
                        }
                }
            }
            return 1;
        }
        // Генерация вируса поменял на void
        public int generate(VitalFunctions whoever)
        {
            if (whoever.immunity < 50 && whoever.infected == false)
            {
                whoever.infected = true;
                whoever.infectionName = name;
                whoever.health = whoever.health - mortality;
                whoever.speed = whoever.speed * 0.1 * mortality;
                whoever.immunity = whoever.immunity - (0.2 * mortality);
                return -1;
            }
            return 1;
        }
        // Изменение координат на координаты больного
        public void changeCoord()
        {
            universalvirusCoord.X = who.Coord.X;
            universalvirusCoord.Y = who.Coord.Y;
            X = universalvirusCoord.X + 1;
            Y = universalvirusCoord.Y + 1;
        }
    }

    [CustomStyle(
        @"new ol.style.Style({ 
            image: new ol.style.Circle({ 
                opacity: 1.0, 
                scale: 1.5, 
                radius: 5, 
                fill: new ol.style.Fill({ 
                    color: 'rgba(0, 255, 0, 0.6)' 
                }), 
                stroke: new ol.style.Stroke({ 
                    color: 'rgb(0, 0, 0)', 
                    width: 1 
                }), 
            }) 
        }); 
        ")]
    public class VitalFunctions : Point// Унаследуем данный данный класс от стандартной точки. 
    {
        // Координаты
        public Coordinate Coord { get; set; }
        // Скорость
        public double speed { get; set; }
        // Здоровье
        public double health { get; set; }
        // Заражен ли
        public bool infected { get; set; }
        // infection
        public string infectionName { get; set; }
        // Тип объекта Human, Animal
        public string objectType { get; set; }
        // Иммунитет
        public double immunity { get; set; }
        // Координаты куда идти
        public Coordinate CoordTo { get; set; }
        // Конструктор для создания нового объекта. 
        public VitalFunctions(Coordinate coordinate, double speedV, double healthV, bool infectedV, double immunityV, string objType, Coordinate ct) : base(coordinate)
        {
            Coord = coordinate;
            speed = speedV;
            health = healthV;
            infected = infectedV;
            immunity = immunityV;
            objectType = objType;
            CoordTo = ct;
        }
        // Течение болезни.
        public int convalescence(HumanVirus hv, AnimalVirus av, UniversalVirus uv, int whatToDo)
        {
            if (infected)
            {
                if (whatToDo == 1)
                {
                    health += ((100 - health) / 2) * hv.mortality * 0.0001;
                    if (100 - health < 5) health = 100;
                    if (100 - health < 0) health = 100;
                    speed += ((1 - speed) / 2) * hv.mortality * 0.004;
                    if (1 - speed < 0.2) speed = 1;
                    if (1 - speed < 0) speed = 1;
                    immunity += ((100 - immunity) / 2) * hv.mortality * 0.003;
                    if (100 - immunity < 3) immunity = 100;
                    if (100 - immunity < 0) immunity = 100;
                    if (health == 100 && speed == 1 && immunity == 100)
                    {
                        infected = false;
                        infectionName = "";
                        return -1;
                    }
                    return 1;
                }
                if (whatToDo == 2)
                {
                    health += ((100 - health) / 2) * hv.mortality * 0.0001;
                    if (100 - health < 5) health = 100;
                    if (100 - health < 0) health = 100;
                    speed += ((1 - speed) / 2) * hv.mortality * 0.004;
                    if (1 - speed < 0.2) speed = 1;
                    if (1 - speed < 0) speed = 1;
                    immunity += ((100 - immunity) / 2) * hv.mortality * 0.003;
                    if (100 - immunity < 3) immunity = 100;
                    if (100 - immunity < 0) immunity = 100;
                    if (health == 100 && speed == 1 && immunity == 100)
                    {
                        infected = false;
                        infectionName = "";
                        return -1;
                    }
                    return 1;
                }
                if (whatToDo == 3)
                {
                    health += ((100 - health) / 2) * hv.mortality * 0.0001;
                    if (100 - health < 5) health = 100;
                    if (100 - health < 0) health = 100;
                    speed += ((1 - speed) / 2) * hv.mortality * 0.004;
                    if (1 - speed < 0.2) speed = 1;
                    if (1 - speed < 0) speed = 1;
                    if (1 - speed == 1)
                    {
                        if (health == 100)
                            speed = 1;
                    }
                    immunity += ((100 - immunity) / 2) * hv.mortality * 0.003;
                    if (100 - immunity < 3) immunity = 100;
                    if (100 - immunity < 0) immunity = 100;
                    if (health == 100 && speed == 1 && immunity == 100)
                    {
                        infected = false;
                        infectionName = "";
                        return -1;
                    }
                    return 1;
                }
                return 1;
            }
            return 1;
        }
        // Передвижение в место
        public int Move()
        {
            //Каноническое уравнение прямой.
            var x1 = Coord.X;
            var y1 = Coord.Y;
            var x2 = CoordTo.X;
            var y2 = CoordTo.Y;
            var x = Coord.X;
            if (x1 < x2)
            {
                x += speed;
                if ((x2 - x1) < speed)
                {
                    x += x2 - x1;
                }
            }
            if (x1 > x2)
            {
                x -= speed;
                if ((x1 - x2) < speed)
                {
                    x += x2 - x1;
                }
            }
            if (Coord.X == x2 && Coord.Y == y2)
            {
                Console.WriteLine("Дошел до точки " + CoordTo);
                return -1;
            }
            X = x;
            Y = ((y2 * (x - x1)) - (y1 * (x - x2))) / (x2 - x1);
            return 1;
        }
    }
}