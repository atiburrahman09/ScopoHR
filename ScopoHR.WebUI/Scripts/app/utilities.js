// takes a list and assumes that it contains objects that has property names with 'Date' in it
// finds those properties and formats it's value to specific date format if provided, to default otherwise
// if user provides a specific set of properties then it will operate on just those properties

function formatDate(dataList, formatString, propertyList) {    
    var format = "YYYY/MM/DD";
    //if format provided use that
    if (formatString != null && formatString.length > 0) {
        format = formatString;
    }

    //verify data exists return empty list otherwise

    if (dataList != null && dataList.length > 0) {
        // if propertyList contains any property then format dates for just those properties
        // this way it will be faster        
        if (propertyList != null && propertyList.length > 0) {
            for (var index in dataList) {
                for (var prop in dataList[index]) {
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
        for (var index in dataList) {
            for (var prop in dataList[index]) {
                if (prop.indexOf("Date") > -1) {
                    dataList[index][prop] != null ? dataList[index][prop] = moment(dataList[index][prop]).format(format) : dataList[index][prop] = "";
                }
            }
        }


        return dataList;
    }
    return [];
}

// takes an array, a perperty and value to compare 
// returns the index of the object in array if found -1 otherwise
function indexOfObjectInArray(array, property, value) {
    for (var i = 0; i < array.length; i++) {
        if (array[i][property] == value) {
            return i;
        }
    }
    return -1;
}

function handleHttpError(err) {

    if (err.status === 400) {
        alertify.error(err.data);
        return;
    }
    else {
        alertify.alert('Unknown error occured, Please contact technical support!');
        return;
    }
}

function getTimeDifference(from, to, format) {
    var oneDay = 24 * 60 * 60 * 1000;
    var mills = Math.round((new Date(to).getTime() - new Date(from).getTime()));
    console.log(mills);
    switch (format) {
        case 'd':
            return parseInt(mills / oneDay);
            break;
        default:
            break;
    }
}

function getTimeDifferenceAbs(from, to, format) {
    var oneDay = 24 * 60 * 60 * 1000;
    var mills = Math.round(Math.abs((new Date(to).getTime() - new Date(from).getTime())));
    console.log(mills);
    switch (format) {
        case 'd':
            return parseInt(mills / oneDay);
            break;
        default:
            break;
    }
}
