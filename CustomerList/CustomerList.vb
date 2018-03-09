﻿
Option Strict On

Imports System.IO

''' <summary>
''' Author Name:    Alfred Massardo
''' Project Name:   CustomerList
''' Date:           05-Jan-2018
''' Description     Application to keep a list of customers and a little information that describes their importance.
''' </summary>

Public Class frmCustomerList

    Private Const positionIdentificationNumber As Integer = 0
    Private Const positionTitle As Integer = 1
    Private Const positionFirstName As Integer = 2
    Private Const positionLastName As Integer = 3
    Private Const positionStatus As Integer = 4

    Private customerList As New SortedList                                 ' collection of all the customerList in the list
    Private currentCustomerIdentificationNumber As String = String.Empty ' current selected customer identification number
    Private editMode As Boolean = False                                 '

    ''' <summary>
    ''' btnEnter_Click - Will validate that the data entered into the controls is appropriate.
    '''                - Once the data is validated a customer object will be create using the  
    '''                - parameterized constructor. It will also insert the new customer object
    '''                - into the customerList collection. It will also check to see if the data in
    '''                - the controls has been selected from the listview by the user. In that case
    '''                - it will need to update the data in the specific customer object and the 
    '''                - listview as well.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub btnEnter_Click(sender As Object, e As EventArgs) Handles btnEnter.Click

        Dim customer As Customer            ' declare a Customer class


        ' validate the data in the form
        If IsValidInput() = True Then

            ' 
            lbResult.Text = "It worked!"

            ' if the current customer identification number has a no value
            ' then this is not an existing item from the listview
            If currentCustomerIdentificationNumber.Trim.Length = 0 Then

                ' create a new customer object using the parameterized constructor
                customer = New Customer(cmbTitles.Text, tbFirstName.Text, tbLastName.Text, chkVIP.Checked)

                ' add the customer to the customerList collection
                ' using the identoification number as the key
                ' which will make the customer object easier to
                ' find in the customerList collection later
                customerList.Add(customer.IdentificationNumber.ToString(), customer)

            Else
                ' if the current customer identification number has a value
                ' then the user has selected something from the list view
                ' so the data in the customer object in the customerList collection
                ' must be updated

                ' so get the customer from the custmers collection
                ' using the selected key
                customer = CType(customerList.Item(currentCustomerIdentificationNumber), Customer)

                ' update the data in the specific object
                ' from the controls
                customer.Title = cmbTitles.Text
                customer.FirstName = tbFirstName.Text
                customer.LastName = tbLastName.Text
                customer.VeryImportantPersonStatus = chkVIP.Checked

            End If


            PopulateListBoxFromSortedList()

            ' Alternate looping strategy
            'For index As Integer = 0 To customerList.Count - 1

            '    ' instantiate a new ListViewItem
            '    customerItem = New ListViewItem()

            '    ' get the customer from the list
            '    customer = CType(customerList(customerList.GetKey(index)), Customer)

            '    ' assign the values to the ckecked control
            '    ' and the subitems
            '    customerItem.Checked = customer.VeryImportantPersonStatus
            '    customerItem.SubItems.Add(customer.IdentificationNumber.ToString())
            '    customerItem.SubItems.Add(customer.Title)
            '    customerItem.SubItems.Add(customer.FirstName)
            '    customerItem.SubItems.Add(customer.LastName)

            '    ' add the new instantiated and populated ListViewItem
            '    ' to the listview control
            '    lvwCustomers.Items.Add(customerItem)

            'Next index

            ' clear the controls
            Reset()



        End If

    End Sub

    ''' <summary>
    ''' Reset - set the controls back to their default state.
    ''' </summary>
    Private Sub Reset()


        tbFirstName.Text = String.Empty
        tbLastName.Text = String.Empty
        chkVIP.Checked = False
        cmbTitles.SelectedIndex = -1
        lbResult.Text = String.Empty

        currentCustomerIdentificationNumber = String.Empty

    End Sub

    ''' <summary>
    ''' IsValidInput - validates the data in each control to ensure that the user has entered apprpriate values
    ''' </summary>
    ''' <returns>Boolean</returns>
    Private Function IsValidInput() As Boolean

        Dim returnValue As Boolean = True
        Dim outputMessage As String = String.Empty

        ' check if the title has been selected
        If cmbTitles.SelectedIndex = -1 Then

            ' If not set the error message
            outputMessage += "Please select the customer's title." & vbCrLf

            ' And, set the return value to false
            returnValue = False

        End If

        ' check if the first name has been entered
        If tbFirstName.Text.Trim.Length = 0 Then

            ' If not set the error message
            outputMessage += "Please enter the customer's first name." & vbCrLf

            ' And, set the return value to false
            returnValue = False

        End If

        ' check if the first name has been entered
        If tbLastName.Text.Trim.Length = 0 Then

            ' If not set the error message
            outputMessage += "Please enter the customer's last name." & vbCrLf

            ' And, set the return value to false
            returnValue = False

        End If

        ' check to see if any value
        ' did not validate
        If returnValue = False Then

            ' show the message(s) to the user
            lbResult.Text = "ERRORS" & vbCrLf & outputMessage

        End If

        ' return the boolean value
        ' true if it passed validation
        ' false if it did not pass validation
        Return returnValue

    End Function

    ''' <summary>
    ''' Event is declared as private because it is only accessible within the form
    ''' The code in the btnReset_Click EventHandler will clear the form and set
    ''' focus back to the input text box. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click

        ' call the rest sub routine
        Reset()

    End Sub


    ''' <summary>
    ''' lvwCustomers_ItemCheck - used to prevent the user from checking the check box in the list view
    '''                        - if it is not in edit mode
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lvwCustomers_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles lvwCustomers.ItemCheck

        ' if it is not in edit mode
        If editMode = False Then

            ' the new value to the current value
            ' so it cannot be set in the listview by the user
            e.NewValue = e.CurrentValue

        End If

    End Sub

    ''' <summary>
    ''' lvwCustomers_SelectedIndexChanged - when the user selected a row in the list it will populate the fields for editing
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lvwCustomers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvwCustomers.SelectedIndexChanged

        ' constant that represents the index of the subitem in the list that
        ' holds the customer identification number 
        Const identificationSubItemIndex As Integer = 1

        ' Get the customer identification number 
        currentCustomerIdentificationNumber = lvwCustomers.Items(lvwCustomers.FocusedItem.Index).SubItems(identificationSubItemIndex).Text

        ' Use the customer identification number to get the customer from the collection object
        Dim customer As Customer = CType(customerList.Item(currentCustomerIdentificationNumber), Customer)

        ' set the controls on the form
        tbFirstName.Text = customer.FirstName               ' get the first name and set the text box
        tbLastName.Text = customer.LastName                 ' get the last name and set the text box
        cmbTitles.Text = customer.Title                     ' get the title and set the combo box
        chkVIP.Checked = customer.VeryImportantPersonStatus ' get the very important person status and set the combo box

        lbResult.Text = customer.GetSalutation()

    End Sub

    Private Sub mnuAbout_Click(sender As Object, e As EventArgs) Handles mnuAbout.Click

        MessageBox.Show("I created this on March 4th 2018")

    End Sub

    Private Sub mnuSave_Click(sender As Object, e As EventArgs) Handles mnuSave.Click

        Dim saveFileDialog As New SaveFileDialog
        Dim fullFilePath As String = String.Empty

        Dim customer As Customer
        Dim output As String = String.Empty

        For Each customerEntry As DictionaryEntry In customerList


            customer = CType(customerEntry.Value, Customer)

            output += customer.GetCommaSeparatedValues() & vbCrLf

        Next

        saveFileDialog.Filter = "txt files (*.txt)|*.txt"

        If saveFileDialog.ShowDialog() = DialogResult.OK Then


            fullFilePath = saveFileDialog.FileName


            save(fullFilePath, output)

        End If



    End Sub

    Private Sub Save(fullFilePath As String, output As String)

        Dim fileStream As New FileStream(fullFilePath, FileMode.Create, FileAccess.Write)

        Dim writeStream As New StreamWriter(fileStream)

        writeStream.Write(output)

        writeStream.Close()


    End Sub

    Private Sub mnuOpen_Click(sender As Object, e As EventArgs) Handles mnuOpen.Click

        Dim openFileDialog As New OpenFileDialog
        Dim fullFilePath As String = String.Empty
        Dim fileContent As String = String.Empty

        openFileDialog.Filter = "txt files (*.txt)|*.txt"

        If openFileDialog.ShowDialog() = DialogResult.OK Then

            fullFilePath = openFileDialog.FileName

            If File.Exists(fullFilePath) Then


                fileContent = OpenFile(fullFilePath)

                PopulateSortedListFromFileContent(fileContent)

                PopulateListBoxFromSortedList()


            End If

        End If


    End Sub


    Private Sub PopulateListBoxFromSortedList()

        Dim customer As Customer
        Dim customerItem As ListViewItem    ' declare a ListViewItem class

        ' set the edit flag to true
        editMode = True

        ' clear the items from the listview control
        lvwCustomers.Items.Clear()

        ' loop through the customerList collection
        ' and populate the list view
        For Each customerEntry As DictionaryEntry In customerList

            ' instantiate a new ListViewItem
            customerItem = New ListViewItem()

            ' get the customer from the list
            customer = CType(customerEntry.Value, Customer)

            ' assign the values to the ckecked control
            ' and the subitems
            customerItem.Checked = customer.VeryImportantPersonStatus
            customerItem.SubItems.Add(customer.IdentificationNumber.ToString())
            customerItem.SubItems.Add(customer.Title)
            customerItem.SubItems.Add(customer.FirstName)
            customerItem.SubItems.Add(customer.LastName)

            ' add the new instantiated and populated ListViewItem
            ' to the listview control
            lvwCustomers.Items.Add(customerItem)

        Next customerEntry

        ' set the edit flag to false
        editMode = False

    End Sub

    Private Sub PopulateSortedListFromFileContent(fileContent As String)

        Dim fileRows As String()
        Dim rowFields As String()

        Dim customer As Customer
        Dim identificationNumber As String = String.Empty
        Dim title As String = String.Empty
        Dim firstName As String = String.Empty
        Dim lastName As String = String.Empty
        Dim status As String = String.Empty

        'vbCrLf
        fileRows = fileContent.Split({vbCr, vbLf}, StringSplitOptions.RemoveEmptyEntries)


        customerList.Clear()

        For Each rowString As String In fileRows

            rowFields = rowString.Split({","c})

            identificationNumber = rowFields(positionIdentificationNumber).Trim
            title = rowFields(positionTitle).Trim
            firstName = rowFields(positionFirstName).Trim
            lastName = rowFields(positionLastName).Trim
            status = rowFields(positionStatus).Trim

            customer = New Customer(identificationNumber, title, firstName, lastName, status)

            customerList.Add(customer.IdentificationNumber.ToString(), customer)

        Next rowString

    End Sub


    Private Function OpenFile(filePath As String) As String

        Dim fileContent As String = String.Empty
        Dim fileStream As New FileStream(filePath, FileMode.Open, FileAccess.Read)

        Dim readStream As New StreamReader(fileStream)

        fileContent = readStream.ReadToEnd()

        readStream.Close()

        Return fileContent

    End Function

    ''' <summary>
    ''' Event is declared as private because it is only accessible within the form
    ''' The code in the mnuExit_Click EventHandler will close the application
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuExit_Click(sender As Object, e As EventArgs) Handles mnuExit.Click

        ' This will close the form
        Me.Close()

    End Sub

    Private Sub mnuNew_Click(sender As Object, e As EventArgs) Handles mnuNew.Click

        customerList.Clear() ' clear the sorted list

        lvwCustomers.Items.Clear() ' clear the list view

    End Sub

End Class




