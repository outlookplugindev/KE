// Submenu.js
/* global Office */
/*import * as Office from 'office-js';*/
import React from 'react';
import MenuItem from './MenuItem';
import $ from 'jquery';
//const { MailboxEnums, mailbox, appointment } = Office;

function Submenu({ items }) {
    const handleMenuItemClick = (menuItemTitle) => {


        //Office.initialize = () => {
            debugger;
            // Access the current item in the mailbox
            const item = Office.context.mailbox.item;
            console.log('Current item:', item);
            console.log('Exchnage Version :' + Office.context.mailbox.diagnostics.hostVersion);
            createFolder();// Woking code
         // getFolderIdsByName('Inbox'); Working code
       // getAllInboxSubfolders();
        //getAccessTokenAndMakeEwsRequest();
       // getExchangeVersion();
        //checkMailboxLocation();

        // Check if the mailbox is hosted in Exchange Online
        //if (Office.context.mailbox.diagnostics.hostName === 'Outlook') {
        //    // Determine the Exchange server version
        //    const exchangeServerVersion = Office.context.mailbox.diagnostics.serverVersion;

        //    console.log('Exchange server version:', exchangeServerVersion);
        //} else {
        //    console.log('Mailbox is not hosted in Exchange Online');
        //}

            // Add event handlers or perform other operations
       // };


        // Check if the mailbox is hosted in Exchange Online
        //if (Office.context.mailbox.diagnostics.hostName === 'Outlook') {
        //    // Determine the Exchange server version
        //    const exchangeServerVersion = Office.context.mailbox.diagnostics.serverVersion;

        //    console.log('Exchange server version:', exchangeServerVersion);
        //} else {
        //    console.log('Mailbox is not hosted in Exchange Online');
        //}
    };
    return (
        <div>
            {items.map((item, index) => (
                <MenuItem key={index} title={item.title} onClick={() => handleMenuItemClick(item.title)} />
            ))}
        </div>
    );
}
// Function to get the ID of the currently selected folder
// Function to get the folder ID by name
// Function to get the folder IDs by name



var I = 0;
function createFolder() {
    I++;
    var folderName = "Ashish Sinha" + I;
    Office.context.mailbox.getCallbackTokenAsync({ isRest: true }, function (asyncResult) {
        if (asyncResult.status === Office.AsyncResultStatus.Succeeded) {
            var accessToken = asyncResult.value;
            debugger;
            var xhr1 = new XMLHttpRequest();
            xhr1.open("POST", "https://localhost:7239/WeatherForecast", true); // Make the request synchronous
            xhr1.setRequestHeader('Access-Control-Allow-Origin', 'https://localhost:44302/post');
            xhr1.setRequestHeader('Access-Control-Allow-Origin', '*');
            xhr1.setRequestHeader('Access-Control-Allow-Origin', '192.168.1.1');
            xhr1.setRequestHeader('Access-Control-Allow-Origin', '192.168.1.13');

            xhr1.setRequestHeader("Content-Type", "application/json");
            xhr1.onreadystatechange = function () {
                if (xhr1.readyState === 4) {
                    if (xhr1.status === 200) {
                        // Request was successful
                        console.log("Token sent to backend");
                        var response = xhr1.responseText;
                        // Handle the response data here
                    } else {
                        debugger;
                        // Request failed
                        //console.error("Error sending token to backend:", xhr.statusText);
                    }
                }
            };
            console.log("Token : "+accessToken)
            var requestData = JSON.stringify({ accessToken: accessToken });
            xhr1.send(requestData);

            return;

            // Create a new XMLHttpRequest object
            var xhr = new XMLHttpRequest();

            // Define the request method, URL, and headers
            xhr.open("POST", "https://outlook.office.com/api/v2.0/me/MailFolders");
            xhr.setRequestHeader("Authorization", "Bearer " + accessToken);
            xhr.setRequestHeader("Content-Type", "application/json");

            // Define the request body
            var requestBody = JSON.stringify({
                "DisplayName": folderName 
            });

            // Define event listener for successful request
            xhr.onload = function () {
                if (xhr.status === 201) {
                    console.log("Folder created successfully:", xhr.responseText);
                    alert("Folder created successfully!");
                } else {
                   // console.error("Error creating folder:", xhr.responseText);
                    //alert("Error creating folder: " + xhr.responseText);
                }
            };

            // Define event listener for error
            xhr.onerror = function () {
               // console.error("Error creating folder:", xhr.responseText);
                //alert("Error creating folder: " + xhr.responseText);
            };

            // Send the request with the defined body
            xhr.send(requestBody);
        } else {
           // console.error("Error obtaining access token:", asyncResult.error);
        }
    });
}
// Function to get the folder IDs by name
// Function to get the folder IDs by name using XMLHttpRequest
function getFolderIdsByName(folderName) {
    // Construct the request URL
    var requestUrl = 'https://outlook.office.com/api/v2.0/me/MailFolders';

    // Get the authorization token asynchronously
    Office.context.mailbox.getCallbackTokenAsync({ isRest: true }, function (result) {
        if (result.status === Office.AsyncResultStatus.Succeeded) {
            var token = result.value;

            // Create XMLHttpRequest object
            var xhr = new XMLHttpRequest();

            // Set up the request
            xhr.open('GET', requestUrl, true);
            xhr.setRequestHeader('Authorization', 'Bearer ' + token);

            // Define what happens on successful data retrieval
            xhr.onload = function () {
                if (xhr.status >= 200 && xhr.status < 300) {
                    // Parse response JSON
                    var data = JSON.parse(xhr.responseText);

                    // Filter the folders by name in the response
                    var matchingFolders = data.value.filter(function (folder) {
                        return folder.DisplayName === folderName;
                    });

                    if (matchingFolders.length > 0) {
                        var folderIds = matchingFolders.map(function (folder) {
                            return folder.Id;
                        });
                        console.log('Folder IDs: ' + folderIds.join(', '));
                        // You can further process the folder IDs as needed
                    } else {
                        console.log('No folders found with the name: ' + folderName);
                    }
                } else {
                    console.log('Request failed with status: ' + xhr.status);
                }
            };

            // Define what happens on error
            xhr.onerror = function () {
                console.log('Error retrieving folders.');
            };

            // Send the request
            xhr.send();
        } else {
            console.log('Error retrieving callback token.');
        }
    });
}
function createFolder_notworking() {
    // Prompt the user for the new folder name
    var folderName = "baba"
    if (folderName) {
        // Use Office JavaScript API to create a new folder
        Office.context.mailbox.makeEwsRequestAsync(
            `<?xml version="1.0" encoding="utf-8"?>
            <soap: Envelope xmlns: soap = "http://schemas.xmlsoap.org/soap/envelope/"
               xmlns: t = "https://schemas.microsoft.com/exchange/services/2006/types">
        <soap:Body>
            <CreateFolder xmlns="https://schemas.microsoft.com/exchange/services/2006/messages">
                <ParentFolderId>
                    <t:DistinguishedFolderId Id="msgfolderroot"/>
                </ParentFolderId>
                <Folders>
                    <t:Folder>
                        <t:DisplayName>Folder1</t:DisplayName>
                    </t:Folder>
                    <t:Folder>
                        <t:DisplayName>Folder2</t:DisplayName>
                    </t:Folder>
                </Folders>
            </CreateFolder>
        </soap:Body>
</soap: Envelope>`,
            function (asyncResult) {
                if (asyncResult.status === Office.AsyncResultStatus.Succeeded) {
                    debugger;
                    //alert("Folder created successfully!");
                } else {
                    debugger;
                    //alert("Error creating folder: " + asyncResult.error.message);
                }
            }
        );
    }
}
// Function to get Exchange Server version
function getExchangeServerVersion() {
    // Create a new instance of ExchangeService object
    var service = new Office.context.mailbox.makeEwsRequestAsync(
        '<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:t="http://schemas.microsoft.com/exchange/services/2006/types">' +
        '  <soap:Header>' +
        '    <t:RequestServerVersion Version="Exchange2013" />' +
        '  </soap:Header>' +
        '  <soap:Body>' +
        '    <GetServerVersionRequest xmlns="http://schemas.microsoft.com/exchange/services/2006/messages" />' +
        '  </soap:Body>' +
        '</soap:Envelope>',
        function (asyncResult) {
            if (asyncResult.status === Office.AsyncResultStatus.Succeeded) {
                debugger;
                var response = asyncResult.value;
                // Parse the SOAP response to extract the Exchange server version
                // This response will contain the version information in the SOAP envelope
                console.log(response);
            } else {
                debugger;
                console.error(asyncResult.error.message);
            }
        }
    );
}

// Function to make EWS request with access token
function makeEwsRequestWithToken(accessToken) {
    // Construct the SOAP envelope for the GetServerVersionRequest
    var soapEnvelope =
        '<?xml version="1.0" encoding="utf-8"?>' +
        '<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:t="http://schemas.microsoft.com/exchange/services/2006/types">' +
        '  <soap:Header>' +
        '    <t:RequestServerVersion Version="Exchange2013" />' +
        '    <t:ExchangeImpersonation>' +
        '      <t:ConnectingSID>' +
        '        <t:SmtpAddress>user@domain.com</t:SmtpAddress>' + // Replace with the user's SMTP address
        '      </t:ConnectingSID>' +
        '    </t:ExchangeImpersonation>' +
        '    <t:Authorization>' +
        '      <t:BearerToken>' + accessToken + '</t:BearerToken>' + // Include the access token
        '    </t:Authorization>' +
        '  </soap:Header>' +
        '  <soap:Body>' +
        '    <GetServerVersionRequest xmlns="http://schemas.microsoft.com/exchange/services/2006/messages" />' +
        '  </soap:Body>' +
        '</soap:Envelope>';

    // Make the EWS request with the access token
    Office.context.mailbox.makeEwsRequestAsync(soapEnvelope, function (asyncResult) {

        if (asyncResult.status === Office.AsyncResultStatus.Succeeded) {
            var response = asyncResult.value;
            // Parse the SOAP response to extract the Exchange server version
            console.log(response);
        } else {
            console.error(asyncResult.error.message);
        }
    });
}
// Function to get all subfolders of the Inbox
function getAllInboxSubfolders() {
    // Construct the request URL
    var requestUrl = 'https://outlook.office.com/api/v2.0/me/mailFolders';

    // Get the authorization token asynchronously
    Office.context.mailbox.getCallbackTokenAsync({ isRest: true }, function (result) {
        if (result.status === Office.AsyncResultStatus.Succeeded) {
            var token = result.value;

            // Make a GET request to retrieve all mail folders
            $.ajax({
                url: requestUrl,
                type: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + token
                },
                success: function (data) {
                    // Filter the folders to include only subfolders of the Inbox
                    var inboxSubfolders = data.value.filter(function (folder) {
                        return folder.ParentFolderId === 'Inbox';
                    });

                    if (inboxSubfolders.length > 0) {
                        console.log('Inbox Subfolders:');
                        inboxSubfolders.forEach(function (folder) {
                            console.log(folder.DisplayName);
                            // You can further process each folder as needed
                        });
                    } else {
                        console.log('No subfolders found in the Inbox.');
                    }
                },
                error: function (error) {
                    console.log('Error retrieving folders: ' + JSON.stringify(error));
                }
            });
        } else {
            console.log('Error retrieving callback token.');
        }
    });
}
// Function to get access token
function getAccessTokenAndMakeEwsRequest() {
    Office.context.mailbox.getCallbackTokenAsync({ isRest: true }, function (result) {
        debugger;
        if (result.status === Office.AsyncResultStatus.Succeeded) {
            var accessToken = result.value;
            // Call the function to make EWS request with access token
            debugger;
            makeEwsRequestWithToken(result.value);
        } else {
            debugger;
            //console.error('Error getting access token:', result.error.message);
        }
    });
}

// Call the function to get access token and make EWS request

// Function to retrieve Exchange server version
// Function to retrieve Exchange server version
function getExchangeVersion() {
    Office.context.mailbox.getCallbackTokenAsync({ isRest: true }, function (result) {
        if (result.status === Office.AsyncResultStatus.Succeeded) {
            var accessToken = result.value;
            var requestUrl = "https://outlook.office.com/api/v2.0/me";

            // Make request to Exchange server
            var xhr = new XMLHttpRequest();
            xhr.open("GET", requestUrl, true);
            xhr.setRequestHeader("Authorization", "Bearer " + accessToken);
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4) {
                    if (xhr.status === 200) {
                        var responseData = JSON.parse(xhr.responseText);
                        var exchangeVersion = responseData.ServerVersion;
                        console.log("Exchange Server Version: " + exchangeVersion);
                    } else {
                        console.error("Failed to retrieve Exchange server version. Status code: " + xhr.status);
                    }
                }
            };
            xhr.send();
        } else {
            console.error("Error retrieving callback token: " + result.error.message);
        }
    });
}

export default Submenu;



