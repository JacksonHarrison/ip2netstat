
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Uri
Imports System.Net.NetworkInformation



Module Module1

    Dim APIKey As String
    Dim arg As New ArrayList
    Dim country_code As String
    Dim filter As String
    Dim colored As Boolean = False
    Dim countrylist As New ArrayList
    Dim filteredcountry As String()
    Dim countryresult As String()
    Dim filtered As Boolean = False
    Dim exist As Boolean = False



    Sub Main(ByVal cmdArgs() As String)
        Dim result As Boolean = False
        Dim ori As Boolean = True

        Try

            Dim args() As String = System.Environment.GetCommandLineArgs()

            Dim selection As Integer = 0
            Dim knet As Boolean = False
            Dim net As Boolean = False
            Dim notRecg As Boolean = False
            Dim counter As Integer = 0
            For x As Integer = 1 To args.Length - 1
                If args(x) = "/?" Then
                    selection = 4
                    ori = False
                ElseIf args(x) = "-k" Then
                    APIKey = args(x + 1)
                    result = True
                    ori = False
                ElseIf args(x) = "-n" Then
                    exist = True
                    ori = False
                ElseIf args(x) = "-f" Then
                    filter = args(x + 1)
                    If args(x + 1) = "-k" Or args(x + 1) = "-n" Or args(x + 1) = "-c" Then
                        ori = False
                        selection = 4
                        Exit For
                    Else
                        filteredcountry = filter.Split("|")
                        filtered = True
                        ori = False
                    End If

                ElseIf args(x) = "-c" Then
                    country_code = args(x + 1)
                    If args(x + 1) = "-k" Or args(x + 1) = "-n" Or args(x + 1) = "-f" Then
                        ori = False
                        selection = 4
                        Exit For
                    Else
                        countryresult = country_code.Split("|")
                        colored = True
                        ori = False
                    End If
                End If
            Next
            If ori = True Then
                DefaultDisplay()
            Else
                If selection = 4 Then
                    ShowHelp()
                Else
                    If result = True Then
                        If exist = False Then
                            ShowStat()
                        Else
                            ShowStat()
                        End If
                    Else
                        If exist = True Then
                            NDisplay()
                        End If

                    End If
                End If

            End If

        Catch e As SystemException
            ShowHelp()

        End Try





    End Sub
    Private Sub DefaultDisplay()
        Dim FilterList As New ArrayList
        Dim list As New ArrayList
        Dim ipProps As System.Net.NetworkInformation.IPGlobalProperties = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties()
        For Each connection As System.Net.NetworkInformation.TcpConnectionInformation In ipProps.GetActiveTcpConnections

            Dim rgx As New Regex("[0-9]+(?:\.[0-9]+){3}(:[0-9]+)?")
            Dim rgx2 As New Regex("127.0.0.1")

            If rgx.IsMatch(connection.LocalEndPoint.ToString) And rgx.IsMatch(connection.RemoteEndPoint.ToString) Then
                If rgx2.IsMatch(connection.LocalEndPoint.ToString) Then
                Else
                    If connection.RemoteEndPoint.ToString.StartsWith("169.254") Then
                    Else
                        If connection.RemoteEndPoint.ToString.StartsWith("10.") Then
                        Else
                            If connection.RemoteEndPoint.ToString.StartsWith("192.168") Then
                            Else
                                Dim rangeStart As Net.IPAddress = Net.IPAddress.Parse("172.16.0.0")
                                Dim rangeEnd As Net.IPAddress = Net.IPAddress.Parse("172.31.255.255")
                                Dim Lip As String = connection.RemoteEndPoint.ToString
                                Dim cut_at As String = ":"
                                Dim cutwhere As Integer = InStr(Lip, cut_at)
                                Dim Ip As String
                                Ip = Lip.Substring(0, cutwhere - 1)
                                Dim hostIPAddress As System.Net.IPAddress = System.Net.IPAddress.Parse(Ip)


                                Dim check As IPAddress = Net.IPAddress.Parse(hostIPAddress.ToString)

                                'get the bytes of the address
                                Dim rbs() As Byte = rangeStart.GetAddressBytes
                                Dim rbe() As Byte = rangeEnd.GetAddressBytes
                                Dim cb() As Byte = check.GetAddressBytes

                                'reverse them for conversion
                                Array.Reverse(rbs)
                                Array.Reverse(rbe)
                                Array.Reverse(cb)

                                'convert them
                                Dim rs As UInt32 = BitConverter.ToUInt32(rbs, 0)
                                Dim re As UInt32 = BitConverter.ToUInt32(rbe, 0)
                                Dim chk As UInt32 = BitConverter.ToUInt32(cb, 0)

                                'check
                                If chk >= rs AndAlso chk <= re Then ' in range

                                Else

                                    list.Add(connection)

                                End If


                            End If
                        End If
                    End If

                End If
            End If

        Next

        Dim pattern As String = "[0-9]+(?:\.[0-9]+){3}:[0-9]"
        Console.WriteLine("{0,-22} {1,-66} {2,-10} ", "Local Address", "Foreign Address", "State")
        Dim string_before As String
        Dim Lstring_before As String
        Dim string_after As String
        For Each item In list

            Dim Lip As String = item.remoteendpoint.ToString
            Dim Fip As String = item.localendpoint.ToString

            Dim cut_at As String = ":"
            Dim cutwhere As Integer = InStr(Lip, cut_at)
            Dim cutwhere2 As Integer = InStr(Fip, cut_at)

            string_before = Lip.Substring(0, cutwhere - 1)
            Lstring_before = Fip.Substring(0, cutwhere2 - 1)
            string_after = Lip.Substring(cutwhere + cut_at.Length - 1)
            Try

                Dim hostIPAddress As IPAddress = IPAddress.Parse(string_before)
                Dim hostInfo As IPHostEntry = Dns.GetHostEntry(hostIPAddress)
                Dim address As IPAddress() = hostInfo.AddressList
                Dim [alias] As [String]() = hostInfo.Aliases

                Console.WriteLine("{0,-22} {1,-60} {2,17} ", Fip, hostInfo.HostName + ":" + string_after, item.state)
            Catch e As FormatException
                Console.WriteLine("FormatException caught!!!")
                Console.WriteLine(("Source : " + e.Source))
                Console.WriteLine(("Message : " + e.Message))
            Catch e As ArgumentNullException
                Console.WriteLine("ArgumentNullException caught!!!")
                Console.WriteLine(("Source : " + e.Source))
                Console.WriteLine(("Message : " + e.Message))
            Catch e As Exception
                Console.WriteLine("{0,-22} {1,-60} {2,17} ", Fip, Lip, item.state)

            End Try


        Next



    End Sub
    Private Sub ShowHelp()
        Console.WriteLine(vbCrLf)
        Console.WriteLine("Displays protocol statistic and current TCP/IP network connections." + vbCrLf)
        Console.WriteLine("ip2netstat [-k API_key] [-n] [-c] [-f] [/?]" + vbCrLf)
        Console.WriteLine("{0,-10} {1,2}", "-k", "After entering '-k' please enter a valid web service API key")
        Console.WriteLine("{0,-10} {1,2}", "", "which you can get from IP2Location (https://www.ip2location.com/web-service).")
        Console.WriteLine("{0,-10} {1,2}", "", "ip2netstat will displays addresses in FQDN form with additional")
        Console.WriteLine("{0,-10} {1,2}", "", "country code and country name.")
        Console.WriteLine("{0,-10} {1,2}", "-n", "Displays addresses in numerical form.")
        Console.WriteLine("{0,-10} {1,2}", "-c", "Works with command '-k' where it will color the result based on the country ")
        Console.WriteLine("{0,-10} {1,2}", "", "code that you have specified.")
        Console.WriteLine("{0,-10} {1,2}", "", "For example, -c ""US"" or -c ""US|MY""")
        Console.WriteLine("{0,-10} {1,2}", "-f", "Works with command '-k' where you can filter the result based on the country")
        Console.WriteLine("{0,-10} {1,2}", "", "code that you have specified.")
        Console.WriteLine("{0,-10} {1,2}", "", "For example, -f ""US"" or -f ""US|MY""")
    End Sub
    Private Sub ShowStat()
        If exist = True Then
            Dim FilterList As New ArrayList
            Dim list As New ArrayList
            Dim ipProps As System.Net.NetworkInformation.IPGlobalProperties = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties()
            For Each connection As System.Net.NetworkInformation.TcpConnectionInformation In ipProps.GetActiveTcpConnections

                Dim rgx As New Regex("[0-9]+(?:\.[0-9]+){3}(:[0-9]+)?")
                Dim rgx2 As New Regex("127.0.0.1")

                If rgx.IsMatch(connection.LocalEndPoint.ToString) And rgx.IsMatch(connection.RemoteEndPoint.ToString) Then
                    If rgx2.IsMatch(connection.LocalEndPoint.ToString) Then
                    Else
                        If connection.RemoteEndPoint.ToString.StartsWith("169.254") Then
                        Else
                            If connection.RemoteEndPoint.ToString.StartsWith("10.") Then
                            Else
                                If connection.RemoteEndPoint.ToString.StartsWith("192.168") Then
                                Else
                                    Dim rangeStart As Net.IPAddress = Net.IPAddress.Parse("172.16.0.0")
                                    Dim rangeEnd As Net.IPAddress = Net.IPAddress.Parse("172.31.255.255")
                                    Dim Lip As String = connection.RemoteEndPoint.ToString
                                    Dim cut_at As String = ":"
                                    Dim cutwhere As Integer = InStr(Lip, cut_at)
                                    Dim Ip As String
                                    Ip = Lip.Substring(0, cutwhere - 1)
                                    Dim hostIPAddress As System.Net.IPAddress = System.Net.IPAddress.Parse(Ip)


                                    Dim check As IPAddress = Net.IPAddress.Parse(hostIPAddress.ToString)

                                    'get the bytes of the address
                                    Dim rbs() As Byte = rangeStart.GetAddressBytes
                                    Dim rbe() As Byte = rangeEnd.GetAddressBytes
                                    Dim cb() As Byte = check.GetAddressBytes

                                    'reverse them for conversion
                                    Array.Reverse(rbs)
                                    Array.Reverse(rbe)
                                    Array.Reverse(cb)

                                    'convert them
                                    Dim rs As UInt32 = BitConverter.ToUInt32(rbs, 0)
                                    Dim re As UInt32 = BitConverter.ToUInt32(rbe, 0)
                                    Dim chk As UInt32 = BitConverter.ToUInt32(cb, 0)

                                    'check
                                    If chk >= rs AndAlso chk <= re Then ' in range

                                    Else
                                        list.Add(connection)
                                    End If


                                End If
                            End If
                        End If

                    End If
                End If

            Next

            Dim pattern As String = "[0-9]+(?:\.[0-9]+){3}:[0-9]"

            Dim string_before As String
            Dim Lstring_before As String
            Console.WriteLine("{0,-22} {1,-23} {2,-17} {3,-20} {4,-20}", "Local Address", "Foreign Address", "State", "Country Code", "Country Name")

            For Each item In list
                Dim Lip As String = item.remoteendpoint.ToString
                Dim Fip As String = item.localendpoint.ToString

                Dim cut_at As String = ":"
                Dim cutwhere As Integer = InStr(Lip, cut_at)
                Dim cutwhere2 As Integer = InStr(Fip, cut_at)

                string_before = Lip.Substring(0, cutwhere - 1)
                Lstring_before = Fip.Substring(0, cutwhere2 - 1)
                Dim request As HttpWebRequest = Nothing
                Dim response As HttpWebResponse = Nothing
                Dim xlist As New ArrayList
                Dim xapiKey As String = APIKey
                Dim data As New Dictionary(Of String, String)
                data.Add("format", "html")
                data.Add("ip", string_before)
                Dim datastr As String = String.Join("&", data.[Select](Function(x) x.Key & "=" & EscapeDataString(x.Value)).ToArray())
                request = Net.WebRequest.Create("http://api.ip2location.com/?ip=" & string_before & "&key=" & xapiKey & "&package=WS2&format=html")
                request.Method = "GET"
                response = request.GetResponse()
                Dim reader As String = New IO.StreamReader(response.GetResponseStream()).ReadToEnd()
                Dim result As String() = reader.Split(";")
                For Each a In result
                    xlist.Add(a)
                Next
                Dim countrycode As String = xlist(0)
                Dim countryname As String = xlist(1)
                If filtered = False Then
                    If colored = True Then
                        If countryresult.Contains(countrycode) Then
                            Console.ForegroundColor = ConsoleColor.Green
                            Console.WriteLine("{0,-22} {1,-23} {2,-17} {3,-20} {4,-20}", item.localendpoint.ToString, item.remoteendpoint.ToString, item.state, countrycode, countryname)
                            Console.ForegroundColor = ConsoleColor.White
                        Else
                            Console.ForegroundColor = ConsoleColor.White
                            Console.WriteLine("{0,-22} {1,-23} {2,-17} {3,-20} {4,-20}", item.localendpoint.ToString, item.remoteendpoint.ToString, item.state, countrycode, countryname)

                        End If
                    Else
                        Console.ForegroundColor = ConsoleColor.White
                        Console.WriteLine("{0,-22} {1,-23} {2,-17} {3,-20} {4,-20}", item.localendpoint.ToString, item.remoteendpoint.ToString, item.state, countrycode, countryname)
                    End If
                Else
                    If colored = True Then
                        If filteredcountry.Contains(countrycode) Then
                            If countryresult.Contains(countrycode) Then
                                Console.ForegroundColor = ConsoleColor.Green
                                Console.WriteLine("{0,-22} {1,-23} {2,-17} {3,-20} {4,-20}", item.localendpoint.ToString, item.remoteendpoint.ToString, item.state, countrycode, countryname)
                                Console.ForegroundColor = ConsoleColor.White
                            Else
                                Console.ForegroundColor = ConsoleColor.White
                                Console.WriteLine("{0,-22} {1,-23} {2,-17} {3,-20} {4,-20}", item.localendpoint.ToString, item.remoteendpoint.ToString, item.state, countrycode, countryname)

                            End If
                        End If
                    Else
                        If filteredcountry.Contains(countrycode) Then
                            Console.ForegroundColor = ConsoleColor.White
                            Console.WriteLine("{0,-22} {1,-23} {2,-17} {3,-20} {4,-20}", item.localendpoint.ToString, item.remoteendpoint.ToString, item.state, countrycode, countryname)

                        End If
                    End If

                End If
            Next
            Console.ForegroundColor = ConsoleColor.White
        Else
            Dim FilterList As New ArrayList
            Dim list As New ArrayList
            Dim ipProps As System.Net.NetworkInformation.IPGlobalProperties = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties()
            For Each connection As System.Net.NetworkInformation.TcpConnectionInformation In ipProps.GetActiveTcpConnections

                Dim rgx As New Regex("[0-9]+(?:\.[0-9]+){3}(:[0-9]+)?")
                Dim rgx2 As New Regex("127.0.0.1")

                If rgx.IsMatch(connection.LocalEndPoint.ToString) And rgx.IsMatch(connection.RemoteEndPoint.ToString) Then
                    If rgx2.IsMatch(connection.LocalEndPoint.ToString) Then
                    Else
                        If connection.RemoteEndPoint.ToString.StartsWith("169.254") Then
                        Else
                            If connection.RemoteEndPoint.ToString.StartsWith("10.") Then
                            Else
                                If connection.RemoteEndPoint.ToString.StartsWith("192.168") Then
                                Else
                                    Dim rangeStart As Net.IPAddress = Net.IPAddress.Parse("172.16.0.0")
                                    Dim rangeEnd As Net.IPAddress = Net.IPAddress.Parse("172.31.255.255")
                                    Dim Lip As String = connection.RemoteEndPoint.ToString
                                    Dim cut_at As String = ":"
                                    Dim cutwhere As Integer = InStr(Lip, cut_at)
                                    Dim Ip As String
                                    Ip = Lip.Substring(0, cutwhere - 1)
                                    Dim hostIPAddress As System.Net.IPAddress = System.Net.IPAddress.Parse(Ip)


                                    Dim check As IPAddress = Net.IPAddress.Parse(hostIPAddress.ToString)

                                    'get the bytes of the address
                                    Dim rbs() As Byte = rangeStart.GetAddressBytes
                                    Dim rbe() As Byte = rangeEnd.GetAddressBytes
                                    Dim cb() As Byte = check.GetAddressBytes

                                    'reverse them for conversion
                                    Array.Reverse(rbs)
                                    Array.Reverse(rbe)
                                    Array.Reverse(cb)

                                    'convert them
                                    Dim rs As UInt32 = BitConverter.ToUInt32(rbs, 0)
                                    Dim re As UInt32 = BitConverter.ToUInt32(rbe, 0)
                                    Dim chk As UInt32 = BitConverter.ToUInt32(cb, 0)

                                    'check
                                    If chk >= rs AndAlso chk <= re Then ' in range

                                    Else
                                        list.Add(connection)
                                    End If


                                End If
                            End If
                        End If

                    End If
                End If

            Next

            Dim pattern As String = "[0-9]+(?:\.[0-9]+){3}:[0-9]"

            Dim string_before As String
            Dim Lstring_before As String
            Console.WriteLine("{0,-22} {1,-55} {2,-18} {3,-20} {4,-20}", "Local Address", "Foreign Address", "State", "Country Code", "Country Name")

            For Each item In list
                Dim Lip As String = item.remoteendpoint.ToString
                Dim Fip As String = item.localendpoint.ToString

                Dim cut_at As String = ":"
                Dim cutwhere As Integer = InStr(Lip, cut_at)
                Dim cutwhere2 As Integer = InStr(Fip, cut_at)

                string_before = Lip.Substring(0, cutwhere - 1)
                Lstring_before = Fip.Substring(0, cutwhere2 - 1)
                Dim request As HttpWebRequest = Nothing
                Dim response As HttpWebResponse = Nothing
                Dim xlist As New ArrayList
                Dim xapiKey As String = APIKey
                Dim data As New Dictionary(Of String, String)
                data.Add("format", "html")
                data.Add("ip", string_before)
                Dim datastr As String = String.Join("&", data.[Select](Function(x) x.Key & "=" & EscapeDataString(x.Value)).ToArray())
                request = Net.WebRequest.Create("http://api.ip2location.com/?ip=" & string_before & "&key=" & xapiKey & "&package=WS2&format=html")
                request.Method = "GET"
                response = request.GetResponse()
                Dim reader As String = New IO.StreamReader(response.GetResponseStream()).ReadToEnd()
                Dim result As String() = reader.Split(";")
                For Each a In result
                    xlist.Add(a)
                Next
                Dim string_after As String
                Dim countrycode As String = xlist(0)
                Dim countryname As String = xlist(1)
                string_before = Lip.Substring(0, cutwhere - 1)
                string_after = Lip.Substring(cutwhere + cut_at.Length - 1)
                Lstring_before = Fip.Substring(0, cutwhere2 - 1)
                string_after = Lip.Substring(cutwhere + cut_at.Length - 1)


                If filtered = False Then
                    If colored = True Then
                        If countryresult.Contains(countrycode) Then
                            Try
                                Dim hostIPAddress As IPAddress = IPAddress.Parse(string_before)
                                Dim hostInfo As IPHostEntry = Dns.GetHostEntry(hostIPAddress)
                                Dim address As IPAddress() = hostInfo.AddressList
                                Console.ForegroundColor = ConsoleColor.Green
                                Console.WriteLine("{0,-22} {1,-55} {2,-18} {3,-20} {4,-20}", item.localendpoint.ToString, hostInfo.HostName + ":" + string_after, item.state, countrycode, countryname)
                                Console.ForegroundColor = ConsoleColor.White
                            Catch ex As Exception
                                Console.ForegroundColor = ConsoleColor.Green
                                Console.WriteLine("{0,-22} {1,-55} {2,-18} {3,-20} {4,-20}", item.localendpoint.ToString, item.remoteendpoint.ToString, item.state, countrycode, countryname)
                                Console.ForegroundColor = ConsoleColor.White
                            End Try

                        Else
                            Try
                                Dim hostIPAddress As IPAddress = IPAddress.Parse(string_before)
                                Dim hostInfo As IPHostEntry = Dns.GetHostEntry(hostIPAddress)
                                Dim address As IPAddress() = hostInfo.AddressList
                                Console.ForegroundColor = ConsoleColor.White
                                Console.WriteLine("{0,-22} {1,-55} {2,-18} {3,-20} {4,-20}", item.localendpoint.ToString, hostInfo.HostName + ":" + string_after, item.state, countrycode, countryname)

                            Catch ex As Exception
                                Console.WriteLine("{0,-22} {1,-55} {2,-18} {3,-20} {4,-20}", item.localendpoint.ToString, item.remoteendpoint.ToString, item.state, countrycode, countryname)
                                Console.ForegroundColor = ConsoleColor.White
                            End Try

                        End If
                    Else
                        Try
                            Dim hostIPAddress As IPAddress = IPAddress.Parse(string_before)
                            Dim hostInfo As IPHostEntry = Dns.GetHostEntry(hostIPAddress)
                            Dim address As IPAddress() = hostInfo.AddressList
                            Console.ForegroundColor = ConsoleColor.White
                            Console.WriteLine("{0,-22} {1,-55} {2,-18} {3,-20} {4,-20}", item.localendpoint.ToString, hostInfo.HostName + ":" + string_after, item.state, countrycode, countryname)
                        Catch ex As Exception
                            Console.WriteLine("{0,-22} {1,-55} {2,-18} {3,-20} {4,-20}", item.localendpoint.ToString, item.remoteendpoint.ToString, item.state, countrycode, countryname)
                            Console.ForegroundColor = ConsoleColor.White
                        End Try

                    End If
                Else
                    If colored = True Then
                        If filteredcountry.Contains(countrycode) Then
                            If countryresult.Contains(countrycode) Then
                                Try
                                    Dim hostIPAddress As IPAddress = IPAddress.Parse(string_before)
                                    Dim hostInfo As IPHostEntry = Dns.GetHostEntry(hostIPAddress)
                                    Dim address As IPAddress() = hostInfo.AddressList
                                    Console.ForegroundColor = ConsoleColor.Green
                                    Console.WriteLine("{0,-22} {1,-55} {2,-18} {3,-20} {4,-20}", item.localendpoint.ToString, hostInfo.HostName + ":" + string_after, item.state, countrycode, countryname)
                                    Console.ForegroundColor = ConsoleColor.White
                                Catch ex As Exception
                                    Console.ForegroundColor = ConsoleColor.Green
                                    Console.WriteLine("{0,-22} {1,-55} {2,-18} {3,-20} {4,-20}", item.localendpoint.ToString, item.remoteendpoint.ToString, item.state, countrycode, countryname)
                                    Console.ForegroundColor = ConsoleColor.White
                                End Try

                            Else
                                Try
                                    Dim hostIPAddress As IPAddress = IPAddress.Parse(string_before)
                                    Dim hostInfo As IPHostEntry = Dns.GetHostEntry(hostIPAddress)
                                    Dim address As IPAddress() = hostInfo.AddressList
                                    Console.ForegroundColor = ConsoleColor.White
                                    Console.WriteLine("{0,-22} {1,-55} {2,-18} {3,-20} {4,-20}", item.localendpoint.ToString, hostInfo.HostName + ":" + string_after, item.state, countrycode, countryname)

                                Catch ex As Exception
                                    Console.WriteLine("{0,-22} {1,-55} {2,-18} {3,-20} {4,-20}", item.localendpoint.ToString, item.remoteendpoint.ToString, item.state, countrycode, countryname)
                                    Console.ForegroundColor = ConsoleColor.White
                                End Try

                            End If
                        End If
                    Else
                        If filteredcountry.Contains(countrycode) Then
                            Try
                                Dim hostIPAddress As IPAddress = IPAddress.Parse(string_before)
                                Dim hostInfo As IPHostEntry = Dns.GetHostEntry(hostIPAddress)
                                Dim address As IPAddress() = hostInfo.AddressList
                                Console.ForegroundColor = ConsoleColor.White
                                Console.WriteLine("{0,-22} {1,-55} {2,-18} {3,-20} {4,-20}", item.localendpoint.ToString, hostInfo.HostName + " :" + string_after, item.state, countrycode, countryname)

                            Catch ex As Exception
                                Console.WriteLine("{0,-22} {1,-55} {2,-18} {3,-20} {4,-20}", item.localendpoint.ToString, item.remoteendpoint.ToString, item.state, countrycode, countryname)
                                Console.ForegroundColor = ConsoleColor.White
                            End Try

                        End If
                    End If
                End If





            Next

        End If




    End Sub
    Private Sub NDisplay()
        Dim FilterList As New ArrayList
        Dim list As New ArrayList
        Dim ipProps As System.Net.NetworkInformation.IPGlobalProperties = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties()
        For Each connection As System.Net.NetworkInformation.TcpConnectionInformation In ipProps.GetActiveTcpConnections

            Dim rgx As New Regex("[0-9]+(?:\.[0-9]+){3}(:[0-9]+)?")
            Dim rgx2 As New Regex("127.0.0.1")

            If rgx.IsMatch(connection.LocalEndPoint.ToString) And rgx.IsMatch(connection.RemoteEndPoint.ToString) Then
                If rgx2.IsMatch(connection.LocalEndPoint.ToString) Then
                Else
                    If connection.RemoteEndPoint.ToString.StartsWith("169.254") Then
                    Else
                        If connection.RemoteEndPoint.ToString.StartsWith("10.") Then
                        Else
                            If connection.RemoteEndPoint.ToString.StartsWith("192.168") Then
                            Else
                                Dim rangeStart As Net.IPAddress = Net.IPAddress.Parse("172.16.0.0")
                                Dim rangeEnd As Net.IPAddress = Net.IPAddress.Parse("172.31.255.255")
                                Dim Lip As String = connection.RemoteEndPoint.ToString
                                Dim cut_at As String = ":"
                                Dim cutwhere As Integer = InStr(Lip, cut_at)
                                Dim Ip As String
                                Ip = Lip.Substring(0, cutwhere - 1)
                                Dim hostIPAddress As System.Net.IPAddress = System.Net.IPAddress.Parse(Ip)


                                Dim check As IPAddress = Net.IPAddress.Parse(hostIPAddress.ToString)

                                'get the bytes of the address
                                Dim rbs() As Byte = rangeStart.GetAddressBytes
                                Dim rbe() As Byte = rangeEnd.GetAddressBytes
                                Dim cb() As Byte = check.GetAddressBytes

                                'reverse them for conversion
                                Array.Reverse(rbs)
                                Array.Reverse(rbe)
                                Array.Reverse(cb)

                                'convert them
                                Dim rs As UInt32 = BitConverter.ToUInt32(rbs, 0)
                                Dim re As UInt32 = BitConverter.ToUInt32(rbe, 0)
                                Dim chk As UInt32 = BitConverter.ToUInt32(cb, 0)

                                'check
                                If chk >= rs AndAlso chk <= re Then ' in range

                                Else
                                    list.Add(connection)
                                End If


                            End If
                        End If
                    End If

                End If
            End If

        Next

        Dim pattern As String = "[0-9]+(?:\.[0-9]+){3}:[0-9]"
        Console.WriteLine("{0,-10} {1,25} {2,25} ", "Local Address", "Foreign Address", "State")
        Dim string_before As String
        Dim Lstring_before As String
        For Each item In list
            Dim Lip As String = item.remoteendpoint.ToString
            Dim Fip As String = item.localendpoint.ToString

            Dim cut_at As String = ":"
            Dim cutwhere As Integer = InStr(Lip, cut_at)
            Dim cutwhere2 As Integer = InStr(Fip, cut_at)

            string_before = Lip.Substring(0, cutwhere - 1)
            Lstring_before = Fip.Substring(0, cutwhere2 - 1)


            Console.WriteLine("{0,-28} {1,-26} {2,-18}", Fip, Lip, item.state)
        Next
    End Sub


End Module
