using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HIB.Outlook.BAL.Managers;
using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.BAL.Repository;

namespace HIB.Outlook.BAL.Tests
{
    [TestClass]
    public class ClientTest
    {
        #region Private Prperties
        IClientRepository _clientRepository = new ClientRepository();
        private readonly IClients _clients = null;
        private readonly string employeeId = "FAGJO1";
        private readonly DateTime lastSyncDate = Convert.ToDateTime("1900-01-01");
        private readonly int pageNumber = 1;
        private readonly int rowsPerPage = 5000;
        private readonly string ipAddress = "1.2.1.5";

        #endregion

        #region Constructor
        public ClientTest()
        {
            _clients = new Clients(_clientRepository);
        }
        #endregion

        #region Methods

        [TestMethod]
        public void SyncClientGetTest()
        {
            var result = _clients.SyncClients(employeeId, lastSyncDate, ipAddress, rowsPerPage, pageNumber);
            Assert.IsNotNull(result);
            // Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod]
        public void SyncClientEmployeeTest()
        {
            var result = _clients.SyncClientEmployee(employeeId, lastSyncDate, ipAddress, rowsPerPage, pageNumber);
            Assert.IsNotNull(result);
            // Assert.AreNotEqual(0, result.Count);
        }
        #endregion
    }
}
