using Microsoft.VisualStudio.TestTools.UnitTesting;
using HIB.Outlook.BAL.Managers;
using System;
using HIB.Outlook.Model;
using System.Collections.Generic;
using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.BAL.Repository;

namespace HIB.Outlook.BAL.Tests
{
    [TestClass]
    public class FavouriteTest
    {
        #region Private Prperties

        private readonly IFavourites _favourites = null;
        IFavouriteRepository _favouriteRepository = new FavouriteRepository();
        private readonly List<FavouriteInfo> favouriteInfoList = new List<FavouriteInfo>()
        {
            new FavouriteInfo { FavourtieName = "fav 1", UniqEmployee = "SHEJO1", UniqEntity = 73939, UniqActivity = 2524668, PolicyYear = "2015", PolicyType = "CBOP", DescriptionType = "Use Email Subj.", Description = "[Reminder - No reply] Submit your pending timesheets", FolderId = 65602, SubFolder1Id = 77078, SubFolder2Id = 77079, ClientAccessibleDate = DateTime.Now.ToString(), CreatedDate = DateTime.Now.ToString() }
        };
       

        #endregion

        #region Constructor
        public FavouriteTest()
        {
            _favourites = new Favourites(_favouriteRepository);
        }
        #endregion

        [TestMethod]
        public void SaveFavouriteDetails()
        {
            var result = _favourites.SaveFavouriteDetails(favouriteInfoList);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
            Assert.AreNotEqual(-1, result);
        }


    }
}
