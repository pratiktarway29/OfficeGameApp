﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTeamForm : Form
    {
        private List<PersonModel> availableTeamMembers = GlobalConfig.Connection.GetPerson_All();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();
        private ITeamRequester callingForm;
        public CreateTeamForm(ITeamRequester caller)
        {
            InitializeComponent();
            callingForm = caller;
            WireUpList();
        }

        private void WireUpList()
        {
            selectTeamMemberDropDown.DataSource = null;
            selectTeamMemberDropDown.DataSource = availableTeamMembers;
            selectTeamMemberDropDown.DisplayMember = "FullName";

            teamMemberListBox.DataSource = null;
            teamMemberListBox.DataSource = selectedTeamMembers;
            teamMemberListBox.DisplayMember = "FullName";
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = new TeamModel();

            t.TeamName = teamNameValue.Text;
            t.TeamMembers = selectedTeamMembers;

            GlobalConfig.Connection.CreateTeam(t);

            callingForm.TeamComplete(t);
            this.Close();
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PersonModel p = new PersonModel();
                p.FirstName = firstNameValue.Text;
                p.Lastname = lastNameValue.Text;
                p.EmailAddress = emailValue.Text;
                p.CellPhoneNumber = cellPhoneValue.Text;

                GlobalConfig.Connection.CreatePerson(p);
                selectedTeamMembers.Add(p);
                WireUpList();

                firstNameValue.Text = "";
                lastNameValue.Text = "";
                emailValue.Text = "";
                cellPhoneValue.Text = "";
            }
            else
            {
                MessageBox.Show("You need to fill in all the fields.");
            }
        }


        private bool ValidateForm()
        {
            if (firstNameValue.Text.Length == 0)
            {
                return false;
            }
            if (lastNameValue.Text.Length == 0)
            {
                return false;
            }
            if (emailValue.Text.Length == 0)
            {
                return false;
            }
            if (cellPhoneValue.Text.Length == 0)
            {
                return false;
            }
            return true;
        }

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)selectTeamMemberDropDown.SelectedItem; 

            if (p != null)
            {
                availableTeamMembers.Remove(p);
                selectedTeamMembers.Add(p);

                WireUpList();
            }
        }

        private void removeSelectedMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)teamMemberListBox.SelectedItem;
            if (p != null)
            {
                selectedTeamMembers.Remove(p);
                availableTeamMembers.Add(p);

                WireUpList();
            }
        }
    }
}
