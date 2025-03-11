using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using MutualFundSimulatorService.Repository.Interface;
using System;

namespace MutualFundSimulatorService.Repository.ConcreteClass
{
    public class DBPatch
    {
        private readonly string _masterConnectionString = @"Server=LAPTOP-HS9AFKH4;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";

        private readonly IRepository _repository;

        public DBPatch(IRepository repository)
        {
            _repository = repository;
        }


        /// <summary>
        /// Creates the MutualFundSimulatorApi database and its required tables if they do not already exist.
        /// </summary>
        public void CreateTablesForMutualFunds()
        { 
            try
            {
                using (SqlConnection masterConnection = new SqlConnection(_masterConnectionString))
                {
                    masterConnection.Open();
                    string createDbQuery = @"
                        IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'MutualFundSimulatorApi')
                        BEGIN
                            CREATE DATABASE MutualFundSimulatorApi
                        END";
                    ExecuteNonQuery(masterConnection, createDbQuery, "Failed to create database MutualFundSimulatorApi");
                }

                using (SqlConnection connection = new SqlConnection(_repository.ConnectionString))
                {
                    connection.Open();

                    string createUsersTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                        BEGIN
                            CREATE TABLE Users (
                                id INT IDENTITY(1,1) PRIMARY KEY,
                                useremail VARCHAR(100) UNIQUE,
                                username VARCHAR(50),
                                userage INT,
                                userpassword VARCHAR(100),
                                userphone VARCHAR(15),
                                walletbalance DECIMAL(18, 2) DEFAULT 0.00
                            )
                        END";
                    ExecuteNonQuery(connection, createUsersTable, "Failed to create Users table");

                    string createFundNAVTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FundNAV')
                        BEGIN
                            CREATE TABLE FundNAV (
                                fundnavid INT IDENTITY(1,1) PRIMARY KEY,
                                fundtype VARCHAR(50),
                                fundname VARCHAR(50),
                                navvalue DECIMAL(10, 2),
                                navdate DATE,
                                CONSTRAINT UC_FundNAV UNIQUE (fundtype, fundname, navdate)
                            )
                        END";
                    ExecuteNonQuery(connection, createFundNAVTable, "Failed to create FundNAV table");

                    string createUserLumpsumPortfolioTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserLumpsumPortfolio')
                    BEGIN
                        CREATE TABLE UserLumpsumPortfolio (
                            lumpsumid INT IDENTITY(1,1) PRIMARY KEY,
                            userid INT,
                            fundname VARCHAR(50),
                            quantity DECIMAL(10, 2),
                            investedamount DECIMAL(10, 2),
                            currentamount DECIMAL(10, 2),
                            lumpsumstartdate DATE,
                            deducted BIT DEFAULT 0,
                            FOREIGN KEY (userid) REFERENCES Users(id)
                        )
                    END";
                    ExecuteNonQuery(connection, createUserLumpsumPortfolioTable, "Failed to create UserLumpsumPortfolio table");

                    string createUserSIPPortfolioTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserSIPPortfolio')
                        BEGIN
                            CREATE TABLE UserSIPPortfolio (
                                sipid INT IDENTITY(1,1) PRIMARY KEY,
                                userid INT,
                                fundname VARCHAR(50),
                                sipamount DECIMAL(10, 2),
                                sipstartdate DATE,
                                sipenddate DATE,
                                totalunits DECIMAL(18, 2),
                                nextinstallmentdate DATE,
                                totalinstallments INT DEFAULT 0,
                                totalinvestedamount DECIMAL(10,2) DEFAULT 0,
                                currentamount DECIMAL(10,2) DEFAULT 0,
                                durationinmonths INT,
                                FOREIGN KEY (userid) REFERENCES Users(id)
                            )
                        END";
                    ExecuteNonQuery(connection, createUserSIPPortfolioTable, "Failed to create UserSIPPortfolio table");

                    string createExpensesTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Expenses')
                        BEGIN
                            CREATE TABLE Expenses (
                                expenseid INT IDENTITY(1,1) PRIMARY KEY,
                                userid INT,
                                fundname NVARCHAR(255),
                                expenseamount DECIMAL(18, 2),
                                expensedate DATE,
                                FOREIGN KEY (userid) REFERENCES Users(id)
                            )
                        END";
                    ExecuteNonQuery(connection, createExpensesTable, "Failed to create Expenses table");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error during database initialization: {ex.Message}");
            }
        }

        /// <summary>
        /// Executes a SQL query on the provided connection, Display an error message if it fails.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <param name="errorMessagePrefix"></param>
        private void ExecuteNonQuery(SqlConnection connection, string query, string errorMessagePrefix)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"{errorMessagePrefix}: {ex.Message}");
            }
        }
    }
}