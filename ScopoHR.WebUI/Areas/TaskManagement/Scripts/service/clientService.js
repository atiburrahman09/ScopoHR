scopoAppServices.service('clientService', function ($http) {

   

    this.getAllClientList = function () {
        return $http.get("/TaskManagement/Client/GetAllClientList");
    };

    this.saveClient = function (client) {
        return $http.post("/TaskManagement/Client/SaveClient", client);
    };


    this.getClientDetailByID = function (clientID) {
        return $http.get("/TaskManagement/Client/GetClientDetailByID", { params: { clientID: clientID } });
    };
});