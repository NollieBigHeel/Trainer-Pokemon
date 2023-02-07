﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// for data connection string
using System.Configuration;
// for SqlConnection
using System.Data.SqlClient;
// for DataTable
using System.Data;

namespace WPF_Pokedex
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Sql connection
        SqlConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();

            // Set up Data connection string
            string connectionString = ConfigurationManager.ConnectionStrings["WPF_Pokedex.Properties.Settings.first_DBConnectionString"].ConnectionString;
            //initialize sql connection
            sqlConnection = new SqlConnection(connectionString);

            // calling the ShowTrainers method
            ShowTrainers();
        }

        // method to show all trainers
        private void ShowTrainers()
        {
            try
            {
                // creates an SQL query
                string query = "select * from Trainer";

                // runs the query onto our sqlConnection
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable trainerTable = new DataTable();

                    // fills trainerTable
                    sqlDataAdapter.Fill(trainerTable);

                    // column of our list is going to be the Name (from table)
                    listTrainers.DisplayMemberPath = "Name";
                    // value is going to be Id
                    listTrainers.SelectedValuePath = "Id";
                    // fill list content from trainerTable
                    listTrainers.ItemsSource = trainerTable.DefaultView;
                }
           

            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            


        }

        // shows the pokemon
        private void ShowPokemon()
        {
            try
            {
                // creates an SQL query
                string query = "select * from Pokemon p inner join TrainerPokemon tp on p.Id = tp.PokemonId where tp.TrainerId = @TrainerId";

                // used for @TrainerId
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // runs our sql command
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    // hands over Id selected in the trainers list box
                    sqlCommand.Parameters.AddWithValue("@TrainerId", listTrainers.SelectedValue);


                    DataTable pokemonTable = new DataTable();

                    // fills trainerTable
                    sqlDataAdapter.Fill(pokemonTable);

                    // column of our list is going to be the Name of pokemon(from table)
                    listPokemon.DisplayMemberPath = "Name";
                    
                    // value is going to be Id
                    listPokemon.SelectedValuePath = "Id";
                    // fill list content from pokemonTable
                    listPokemon.ItemsSource = pokemonTable.DefaultView;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }
        private void listTrainers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowPokemon();
        }
    }
}
