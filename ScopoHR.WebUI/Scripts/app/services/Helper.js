scopoAppServices.service('Helper', function ($http) {
    const DEV = "http://localhost:55820/";
    const PROD = 'http://182.160.125.178:777/';
    const TEST = 'http://182.160.125.178:778/';

    this.ServerRoot = DEV;

    this.PlaceholderImage = "../../Content/placeholder.png";  
    this.getFileUploadTarget = () => { return this.ServerRoot + "api/fileupload/documentsupload/upload"; }
    this.getDocumentLocations = () => {
        return $http.get(this.ServerRoot + "api/fileupload/documentsupload/getdocumentlocations");
    }

    this.getDocumentCategories = () => {
        return $http.get(this.ServerRoot + "api/fileupload/documentsupload/getdocumentcategories");
    }

    this.FormatDate = (dataList, formatString, propertyList) => {    
        let format = "YYYY/MM/DD";
        //if format provided use that
        if (formatString != null && formatString.length > 0) {
            format = formatString;
        }

        //verify data exists return empty list otherwise

        if (dataList != null && dataList.length > 0) {
            // if propertyList contains any property then format dates for just those properties
            // this way it will be faster        
            if (propertyList != null && propertyList.length > 0) {
                for (let index in dataList) {
                    for (let prop in dataList[index]) {
                        for (i in propertyList) {
                            if (propertyList[i] === prop) {
                                dataList[index][prop] != null ? dataList[index][prop] = moment(dataList[index][prop]).format(format) : dataList[index][prop] = "";
                            }
                        }
                    }
                }
                return dataList;
            }
            // if propertyList is empty or null then look for properties with 'Date'
            for (let index in dataList) {
                for (let prop in dataList[index]) {
                    if (prop.indexOf("Date") > -1) {
                        dataList[index][prop] != null ? dataList[index][prop] = moment(dataList[index][prop]).format(format) : dataList[index][prop] = "";
                    }
                }
            }            
            return dataList;
        }
        return [];
    }

});