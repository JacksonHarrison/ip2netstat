ip2netstat
==========
ip2netstat is a console program that allows users to display protocol statistics and current
TCP/IP network connections along with country geolocation data.

Pre-requisites
==============
A web service API Key is required to display country geolocation data like country codes and country names.
Sign up at https://www.ip2location.com/web-service to get a web service API Key.

Usage parameters
================
### [-k API Key]
This parameter is used to display the address along with the geolocated country code and country name.

### [-n]
This parameter is used to display the address in numerical form.

### [-c]
This parameter works with "-k API Key" where you can color the result based on the country code(s) that
you have specified.

For example, -c "US" or -c "US|MY"

### [-f]
This parameter works with "-k API Key" where you can filter the result based on the country code(s) that 
you have specified.

For example, -f "US" or -f "US|MY"
