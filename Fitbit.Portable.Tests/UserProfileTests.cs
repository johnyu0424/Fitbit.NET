﻿using System;
using System.Net;
using System.Net.Http;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class UserProfileTests
    {
        [Test]
        public async void GetUserProfileAsync_Success()
        {
            string content = "UserProfile.json".GetContent();
         
            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/profile.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetUserProfileAsync();
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            var user = response.Data;

            Assert.AreEqual(1, fakeResponseHandler.CallCount);

            ValidateSingleUserProfile(user);
        }

        [Test]
        public async void GetUserProfileAsync_Failure_Errors()
        {
            string content = "UserProfile.json".GetContent();
         
            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/ZXAS11/profile.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetFriendsAsync();
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
        }

        [Test]
        public void Can_Deserialize_Profile()
        {
            string content = "UserProfile.json".GetContent();
            var deserializer = new JsonDotNetSerializer {RootProperty = "user"};

            var result = deserializer.Deserialize<UserProfile>(content);

            ValidateSingleUserProfile(result);
        }

        private void ValidateSingleUserProfile(UserProfile user)
        {
            Assert.IsNotNull(user);
            Assert.AreEqual("http://www.fitbit.com/images/profile/defaultProfile_100_male.gif", user.Avatar);
            Assert.AreEqual("http://www.fitbit.com/images/profile/defaultProfile_150_male.gif", user.Avatar150);
            Assert.AreEqual("GB", user.Country);
            Assert.AreEqual(new DateTime(1983, 1, 28), user.DateOfBirth);
            Assert.AreEqual("Adam", user.DisplayName);
            Assert.AreEqual("en_US", user.DistanceUnit);
            Assert.AreEqual("XXXXXX", user.EncodedId);
            Assert.AreEqual("en_US", user.FoodsLocale);
            Assert.AreEqual("Fitbit User", user.FullName);
            Assert.AreEqual(Gender.MALE, user.Gender);
            Assert.AreEqual("METRIC", user.GlucoseUnit);
            Assert.AreEqual(170.2, user.Height);
            Assert.AreEqual("en_US", user.HeightUnit);
            Assert.AreEqual("en_GB", user.Locale);
            Assert.AreEqual(new DateTime(2014, 1, 6), user.MemberSince);
            Assert.AreEqual(-25200000, user.OffsetFromUTCMillis);
            Assert.AreEqual("SUNDAY", user.StartDayOfWeek);
            Assert.AreEqual(88.5, user.StrideLengthRunning);
            Assert.AreEqual(70.60000000000001, user.StrideLengthWalking);
            Assert.AreEqual("Europe/London", user.Timezone);
            Assert.AreEqual("METRIC", user.WaterUnit);
            Assert.AreEqual(79.3, user.Weight);
            Assert.AreEqual("METRIC", user.WeightUnit);
        }
    }
}