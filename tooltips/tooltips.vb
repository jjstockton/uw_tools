Private Function equal(a1() As String, a2() As String) As Boolean

    equal = True

    For i = LBound(a1) To UBound(a2)
        equal = equal And (a1(i) = a2(i))
    Next i
End Function

Private Function getOffered(subject As String, courseNum As String) As String

    Const sensitivity = 1

    Dim month_of_year As Integer: month_of_year = 1
    Dim month As Integer
    Dim curr_year As Integer: curr_year = year(Date) - sensitivity - 1
    Dim offered(2) As String
    Dim prev_offered(2) As String
    Dim count As Integer: count = 0
    
    
    month = month_of_year
    
    While curr_year < 2017 Or month_of_year < 4
    
        If month_of_year = 1 Then
        
            
        End If
            
        Dim session As String
        session = "1" & Right(curr_year, 2) & month_of_year
        
        'MsgBox session
        
        Set ie = New InternetExplorer
        ie.Visible = False
        ie.navigate "https://info.uwaterloo.ca/cgi-bin/cgiwrap/infocour/salook.pl?sess=" & session & "&level=under&subject=" & subject & "&cournum=" & courseNum
      
        Do While ie.readyState <> 4
            Application.StatusBar = "Loading"
            DoEvents
        Loop
        Set html = ie.document
        
        Dim text As String
        text = html.DocumentElement.innerText
    
        Application.StatusBar = ""
        ie.Quit
        
        If InStr(text, "Sorry, but your query had no matches.") = 0 Then
            'MsgBox "offered"
            If month_of_year = 1 Then
                offered(1) = "W"
            ElseIf month_of_year = 5 Then
                offered(2) = "S"
            ElseIf month_of_year = 9 Then
                offered(0) = "F"
            End If
        End If
        
        month = month + 4
        month_of_year = month Mod 12
        curr_year = curr_year + (1 / month_of_year)
        
        If month_of_year = 1 Then
            
            
            If equal(offered, prev_offered) Then
                count = count + 1
            Else
                count = 0
            End If
            For i = LBound(offered) To UBound(offered)
                prev_offered(i) = offered(i)
                offered(i) = ""
            Next i
            
        End If
    Wend
    
    If count < sensitivity Then
        getOffered = "Offered: ???"
    ElseIf (prev_offered(0) & prev_offered(1) & prev_offered(2)) = "" Then
        getOffered = "Offered: Never"
    Else
    
        Dim output As String
        
        For Each element In prev_offered
            If element <> "" Then
                output = output + element + ","
            End If
        Next element
        
        output = Left(output, Len(output) - 1)
    
        getOffered = "Offered: " & output & "*"
    End If
End Function

Sub updateTooltip(Myrange As Range)

    'MsgBox Myrange.text
    Dim regEx As New RegExp
    Dim regEx2 As New RegExp
    Dim strPattern As String
    Dim strInput As String
    Dim strReplace As String
    
    Dim matches
    Dim matches2
    Dim ie As InternetExplorer
    Dim html As HTMLDocument
    
    Dim offered
    
    'Dim Name As String
    'Dim Info As String
        
    For Each C In Myrange
        'MsgBox C.text
        If IsEmpty(C.Value) Then
            C.Validation.Delete
        End If
        strPattern = "(^[A-Z]{2,5}) ([0-9]{2,3}[A-Z]?)\?$"

        If strPattern <> "" Then
            strInput = C.Value
            strReplace = "$1"

            With regEx
                .Global = True
                .MultiLine = True
                .IgnoreCase = True
                .Pattern = strPattern
            End With

            If regEx.test(strInput) Then
            
                Set ie = New InternetExplorer
                ie.Visible = False
                
                Set matches = regEx.Execute(strInput)
                'MsgBox "http://www.ucalendar.uwaterloo.ca/1617/COURSE/course-" & matches(0).SubMatches(0) & ".html"
                ie.navigate "http://www.ucalendar.uwaterloo.ca/1617/COURSE/course-" & matches(0).SubMatches(0) & ".html"
                'Wait until IE is done loading page
                With C.Validation
                    .Delete
                    .Add Type:=xlValidateInputOnly
                    .InputTitle = "Loading..."
                    .InputMessage = "Loading..."
                End With
                Do While ie.readyState <> 4
                    Application.StatusBar = "Loading"
                    DoEvents
                Loop
                Set html = ie.document
                
                
                Dim text As String
                text = html.DocumentElement.innerText
                
                With regEx2
                    .Global = True
                    .MultiLine = True
                    .IgnoreCase = True
                  
                    'MsgBox "(" & C.text & " [A-Z]{3}[,'-.:0-9A-Z ]*)\s*([A-Z0-9 ]*)\s*([,;/'-.:0-9A-Z ]*)\s*\[([A-Z,: ]*)\]?"
                
                    '.Pattern = "(" & C.text & " [A-Z]{3}[,'-.:0-9A-Z ]*)\s*([-A-Z0-9,() ]*)\s*([,;/'-.:0-9A-Z ]*)\s*(\[([A-Z,: ]*)\])?"
                    .Pattern = C.text & " ([A-Z]{3},?)+ [0-9.]{4}Course ID: [0-9]+\s*([\w,\- ]*)\s*[^\[\n]*\n*(\[.*?(Offered.*)\])?"
                End With
            
                Application.StatusBar = ""
                
                ie.Quit
                
            
                If regEx2.test(text) Then
                    'MsgBox regEx.Replace(text, "$2")
                    Set matches2 = regEx2.Execute(text)
                    GetStringInParens = matches2(0)
                    'MsgBox GetStringInParens
                
                
                    courseName = matches2(0).SubMatches(1)
                    
                    If Len(courseName) > 32 Then
                        courseName = Replace(courseName, "Engineering", "Eng")
                        courseName = Replace(courseName, "Introduction", "Intro")
                        courseName = Replace(courseName, "Linear Algebra", "Lin Alg")
                        courseName = Replace(courseName, "Mathematics", "Math")
                        courseName = Replace(courseName, "Nanotechnology Engineers", "NEs")
                        
                    End If
                    
                    If Len(courseName) > 32 Then
                        courseName = Replace(courseName, "Electrical and Computer Eng", "ECE")
                        courseName = Replace(courseName, "Electrical and Computer Engineers", "ECEs")
                        courseName = Replace(courseName, "Electrical Engineers", "EEs")
                        courseName = Replace(courseName, "Computer Engineers", "CEs")
                        courseName = Replace(courseName, "Electrical Eng", "EE")
                        courseName = Replace(courseName, "Computer Eng", "CE")
                        courseName = Left(courseName, 32)
                    End If
                
                    With C.Validation
                        .Delete
                        .Add Type:=xlValidateInputOnly
                        .InputTitle = courseName
                        .InputMessage = "Loading..."
                    End With
                    
                    If matches2(0).SubMatches(3) = "" Then
                        CourseInfo = getOffered(matches(0).SubMatches(0), matches(0).SubMatches(1))
                    Else
                        CourseInfo = Left(matches2(0).SubMatches(3), 255)
                    End If
                    
                    C.Validation.InputMessage = CourseInfo & " "
                    
                    Else
                    C.Validation.Delete
                End If
            End If
        End If
    Next

End Sub

Private Sub Worksheet_Change(ByVal Target As Range)
    If Not Intersect(Target, Range("A1:AAA702")) Is Nothing Then updateTooltip (ActiveSheet.Range(Target.Address))
End Sub