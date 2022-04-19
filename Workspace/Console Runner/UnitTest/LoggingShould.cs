﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Console_Runner.Logging;
using Console_Runner.Logging.Testing;

namespace UnitTest
{
    public class LoggingShould
    {
        private readonly ILogGateway _logGateway = new MemLogGateway();
        private readonly IUserIDGateway _userIDGateway = new MemUserIdentifierGateway();

        [Fact]
        public async void WriteLogsProperly()
        {
            LogService logger = new LogService(_logGateway, _userIDGateway);
            var firstLog = ("example@email.com", LogLevel.Info, Category.Server, DateTime.Now.ToUniversalTime(), "Example log message.");
            var secondLog = ("example@email.com", LogLevel.Info, Category.Server, DateTime.Now.ToUniversalTime(), "Another example log message.");
            var thirdLog = ("anotherexample@email.com", LogLevel.Error, Category.Data, DateTime.Parse("2012-09-10 12:10:15").ToUniversalTime(), "Another example log message.");

            Log firstSuccess = await logger.LogAsync(firstLog.Item1, firstLog.Item2, firstLog.Item3, firstLog.Item4, firstLog.Item5);
            Log secondSuccess = await logger.LogAsync(secondLog.Item1, secondLog.Item2, secondLog.Item3, secondLog.Item4, secondLog.Item5);
            Log thirdSuccess = await logger.LogAsync(thirdLog.Item1, thirdLog.Item2, thirdLog.Item3, thirdLog.Item4, thirdLog.Item5);

            ((MemLogGateway)_logGateway).PrintAllLogs();

            Assert.Equal(await _userIDGateway.GetUserHashAsync(firstLog.Item1), await _userIDGateway.GetUserHashAsync(firstSuccess.UserIdentifier.UserId));
            Assert.Equal(firstLog.Item2, firstSuccess.LogLevel);
            Assert.Equal(firstLog.Item3, firstSuccess.Category);
            Assert.Equal(firstLog.Item4, firstSuccess.Timestamp);
            Assert.Equal(firstLog.Item5, firstSuccess.Message);

            Assert.Equal(await _userIDGateway.GetUserHashAsync(secondLog.Item1), await _userIDGateway.GetUserHashAsync(secondSuccess.UserIdentifier.UserId));
            Assert.Equal(secondLog.Item2, secondSuccess.LogLevel);
            Assert.Equal(secondLog.Item3, secondSuccess.Category);
            Assert.Equal(secondLog.Item4, secondSuccess.Timestamp);
            Assert.Equal(secondLog.Item5, secondSuccess.Message);

            Assert.Equal(await _userIDGateway.GetUserHashAsync(thirdLog.Item1), await _userIDGateway.GetUserHashAsync(thirdSuccess.UserIdentifier.UserId));
            Assert.Equal(thirdLog.Item2, thirdSuccess.LogLevel);
            Assert.Equal(thirdLog.Item3, thirdSuccess.Category);
            Assert.Equal(thirdLog.Item4, thirdSuccess.Timestamp);
            Assert.Equal(thirdLog.Item5, thirdSuccess.Message);
        }
        
        [Fact]
        public async void ThrowsOperationCanceledExceptionOnTimeout()
        {
            LogService logger = new LogService(_logGateway, _userIDGateway);
            var firstLog = ("example@email.com", LogLevel.Info, Category.Server, DateTime.Now.ToUniversalTime(), "Example log message.");
            Func<Task> attempt = async () => await logger.LogAsync(firstLog.Item1, firstLog.Item2, firstLog.Item3, firstLog.Item4, firstLog.Item5, 0);
            await Assert.ThrowsAsync<OperationCanceledException>(attempt);
            Func<Task> secondAttempt = async () => await logger.LogAsync(firstLog.Item1, firstLog.Item2, firstLog.Item3, firstLog.Item4, firstLog.Item5, 10);
            await Assert.ThrowsAsync<OperationCanceledException>(attempt);
        }
        
    }
}
