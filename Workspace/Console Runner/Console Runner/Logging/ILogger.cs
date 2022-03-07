using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Logging
{
    /// <summary>
    /// Logger interface class, holds empty methods names
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Function that will take a formatted string as input and send it to our log storage
        /// </summary>
        /// <param name="toLog">The string that will be logged in the data store</param>
        /// <returns>A bool value indicating whether the action was successful</returns>
        public bool Log(string toLog);
        /// <summary>
        /// Logging funciton to format user log in
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <returns>A string formated as an account login log</returns>
        public bool LogLogin(string category, string pageName, bool isSuccess, string failCase, string user);
        /// <summary>
        /// Logging funciton to format account deactivation
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <returns></returns>
        /// <param name="target">The account that is being targeted for deactivation</param>
        /// <returns>A string formated as an account deactivation log</returns>
        public bool LogAccountDeactivation(string category, string pageName, bool isSuccess, string failCase, string user, string target);
        /// <summary>
        /// Logging funciton to format account enabling 
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <returns></returns>
        /// <param name="target">The account that is being targeted for enabling</param>
        /// <returns>Returns a string formated as an account enabling log</returns>
        public bool LogAccountEnabling(string category, string pageName, bool isSuccess, string failCase, string user, string target);
        /// <summary>
        /// Logging funciton to format account promotion
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <returns></returns>
        /// <param name="promoted">User whom is being promoted</param>
        /// <returns>A string formated as an account promotion log</returns>
        public bool LogAccountPromote(string category, string pageName, bool isSuccess, string failCase, string user, string promoted);
        /// <summary>
        /// Logging funciton to format account creation
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action, in this case, the account being created</param>
        /// <returns>A string formatted as an account creation log</returns>
        public bool LogAccountCreation(string category, string pageName, bool isSuccess, string failCase, string user);
        /// <summary>
        /// Logging funciton to format account deletion
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <returns>A string formatted as an account deletion log</returns>
        public bool LogAccountDeletion(string category, string pageName, bool isSuccess, string failCase, string user);

        /// <summary>
        /// Logging funciton to format account name change
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="prevName">User's previous name</param>
        /// <param name="newName">User's name to be changed to</param>
        /// <returns>A string formatted as an account name change log</returns>
        public bool LogAccountNameChange(string category, string pageName, bool isSuccess, string failCase, string user, string prevName, string newName);
        /// <summary>
        /// Logging funciton to format account email change
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="prevEmail">User's previous email</param>
        /// <param name="newEmail">User's email to be changed to</param>
        /// <returns>A string formatted as an account email change log</returns>
        public bool LogAccountEmailChange(string category, string pageName, bool isSuccess, string failCase, string user, string prevEmail, string newEmail);
        /// <summary>
        /// Logging funciton to format account password change
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <returns>A string formatted as an account password change log</returns>
        public bool LogAccountPasswordChange(string category, string pageName, bool isSuccess, string failCase, string user);
        /// <summary>
        /// Logging funciton to format account food flag change
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="added">Any food flags that have been added by the user</param>
        /// <param name="removed">Any food flags that have been removed by the user</param>
        /// <returns>A string formatted as an account food flag change</returns>
        public bool LogAccountFlagChange(string category, string pageName, bool isSuccess, string failCase, string user, string[] added, string[] removed);
        /// <summary>
        /// Logging funciton to format account data request log
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="sendTo">The email that the data is to be sent</param>
        /// <returns>A string formatted as an account data request log</returns>
        public bool LogAccountDataRequest(string category, string pageName, bool isSuccess, string failCase, string user, string sendTo);
        /// <summary>
        /// Logging funciton to format account AMR change
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="from">User's previous AMR values</param>
        /// <param name="to">User's new AMR values</param>
        /// <returns>A string formatted as an account amr change</returns>
        public bool LogAccountAmrChange(string category, string pageName, bool isSuccess, string failCase, string user, string[] from, string[] to);
        /// <summary>
        /// Logging funciton to format an account creating a review
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="product">The product the review is for</param>
        /// <param name="rating">The rating the user gave the product</param>
        /// <param name="text">The written review of the product</param>
        /// <returns>A string formatted to be a review log</returns>
        public bool LogReview(string category, string pageName, bool isSuccess, string failCase, string user, string product, int rating, string text);
        /// <summary>
        /// Logging funciton to format account history additions
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="product">The product that is being added to the user's history</param>
        /// <param name="index">The index that this product is being inserted to of the user's history</param>
        /// <returns>A string formatted as an account history log</returns>
        public bool LogHistory(string category, string pageName, bool isSuccess, string failCase, string user, string product, int index);
        /// <summary>
        /// Logging funciton to format account scan upload
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="product">The product that has been scanned and uploaded</param>
        /// <returns>A string formatted as a user scan upload</returns>
        public bool LogScanUpload(string category, string pageName, bool isSuccess, string failCase, string user, string product);
        /// <summary>
        /// Logging funciton to create a generic log
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="info">Any info that is relavent to the generic log</param>
        /// <returns>Returns a string formatted as a generic log</returns>
        public bool LogGeneric(string category, string pageName, bool isSuccess, string failCase, string user, string info);
    }
}
