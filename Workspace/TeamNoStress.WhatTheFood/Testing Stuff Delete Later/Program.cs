// See https://aka.ms/new-console-template for more information
using TeamNoStress.WhatTheFood.DataAccessLayer;

SQLDataAccessObject test = new SQLDataAccessObject();
test.GetAllUserHistory("user");
Console.WriteLine("Hello, World!");
