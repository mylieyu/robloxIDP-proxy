# roblox-proxy

This project will allow you proxy web calls from Roblox to Google Sheets via Azure Functions.

Before you begin, make sure you have the following:

- [.NET 6](https://dotnet.microsoft.com/en-us/download)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
- [Visual Studio Code](https://code.visualstudio.com/Download)
- [Azure Subscription](https://azure.microsoft.com/en-us/free/students/)
- Your Google Sheet App URL

To deploy this project, open Azure Cloud Shell and run the following commands

```bash
# create a resource group
az group create -n MyResourceGroup -l westus

# name must be unique
az storage account create --name mystorageaccount0525 \
  -g MyResourceGroup \
  -l westus \
  --sku Standard_LRS

# name must be unique
az functionapp create --consumption-plan-location westus \
  --name MyUniqueAppName0525 \
  --os-type Windows \
  --resource-group MyResourceGroup \
  --runtime dotnet \
  --runtime-version 6 \
  --functions-version 4 \
  --storage-account mystorageaccount0525

# add your google api to the function app settings
googleurl='PUT_YOUR_GOOGLE_SHEET_URL_HERE'

az functionapp config appsettings set --name MyUniqueAppName0525 \
  --resource-group MyResourceGroup \
  --settings "GoogleSheetApiUrl=$googleurl"

# build your project
dotnet build
dotnet publish -C Release

# package your project
cd bin/Release/net6.0/publish && zip -r publish.zip .

# publish your project to azure functions
az functionapp deploy --resource-group MyResourceGroup \
  --name MyUniqueAppName0525 \
  --src-path publish.zip \
  --clean true \
  --restart true \
  --type zip
 
# delete files
rm publish.zip && cd -

# retrieve function key
code=$(az functionapp function keys list --resource-group MyResourceGroup \
  --name MyUniqueAppName0525 \
  --function-name roblox \
  --query "default" \
  -o tsv)

# retrieve function url
url=$(az functionapp function show --function-name roblox \
  --name MyUniqueAppName0525 \
  --resource-group MyResourceGroup \
  --query "invokeUrlTemplate" \
  --output tsv)
  
# output the entire url
echo "$url?code=$code"

# now test the api call using a REST client
```
