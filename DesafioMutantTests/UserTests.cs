using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Json;
using DesafioMutant.UserData;
using Newtonsoft.Json;
using DesafioMutant;
using System.Collections.Generic;

namespace DesafioMutantTests
{
    [TestClass]
    public class UserTests
    {
        /// <summary>
        /// Verifica se retorno da API é um JSON válido.
        /// </summary>
        [TestMethod]
        public void ValidateJSONReturn()
        {
            var x = DesafioMutant.Dados.BaixarDados("https://jsonplaceholder.typicode.com/users");
            var json = JsonValue.Parse(x);
        }

        /// <summary>
        /// Verifica se usuário mockado é inserido no banco corretamente
        /// </summary>
        [TestMethod]
        public void ValidateDBInsert()
        {
            List<User> list = new List<User>();
            User mockUser = new User();

            mockUser.Id = 99;
            mockUser.Name = "Name Lastname";
            mockUser.Username = "nlastname";
            mockUser.Email = "email@email.email";
            mockUser.Phone = "555-555555";
            mockUser.Website = "site.com";

            Address address = new Address();
            address.Street = "streeeeeeeeeeeeeet";
            address.Suite = "Suite 555";
            address.City = "River of January";
            address.Zipcode = "666666-666";

            Geo geo = new Geo();
            geo.Lat = "-66.6666";
            geo.Lng = "55.5555";

            address.Geo = geo;
            mockUser.Address = address;

            Company company = new Company();
            company.Name = "lexcorp";
            company.CatchPhrase = "oi";
            company.BS = "isso";

            mockUser.Company = company;

            list.Add(mockUser);

            var userJSON = JsonConvert.SerializeObject(list, Formatting.Indented);
            var userString = userJSON.ToString();

            var x = Dados.SalvarDados(userString);

            Assert.IsTrue(x > 0);
        }

        /// <summary>
        /// Verifica se usuário hospedado em apartamento é inserido no banco.
        /// </summary>
        [TestMethod]
        public void ValidateDBInsert_SuiteOnly()
        {
            int expected = 0;
            List<User> list = new List<User>();
            User mockUser = new User();

            mockUser.Id = 99;
            mockUser.Name = "Name Lastname";
            mockUser.Username = "nlastname";
            mockUser.Email = "email@email.email";
            mockUser.Phone = "555-555555";
            mockUser.Website = "site.com";

            Address address = new Address();
            address.Street = "streeeeeeeeeeeeeet";
            address.Suite = "Apt. 555";
            address.City = "River of January";
            address.Zipcode = "666666-666";

            Geo geo = new Geo();
            geo.Lat = "-66.6666";
            geo.Lng = "55.5555";

            address.Geo = geo;
            mockUser.Address = address;

            Company company = new Company();
            company.Name = "lexcorp";
            company.CatchPhrase = "oi";
            company.BS = "isso";

            mockUser.Company = company;

            list.Add(mockUser);

            var userJSON = JsonConvert.SerializeObject(list, Formatting.Indented);
            var userString = userJSON.ToString();

            Assert.AreEqual(Dados.SalvarDados(userString), expected);
        }
    }
}
