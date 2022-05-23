
var scopoApp = angular.module('scopoApp', ['scopoApp.Controllers', 'scopoApp.Filters', 'ngCkeditor', 'ngAlertify', 'flow', 'mentio', 'ui.bootstrap.datetimepicker',
    'ui.select', 'ngSanitize', 'ui.directives', 'ui.filters', 'angucomplete-alt', 'ui.bootstrap', 'angular-loading-bar']);
 
var scopoAppControllers = angular.module('scopoApp.Controllers', ['scopoApp.Services']);
var scopoAppServices = angular.module('scopoApp.Services', []);
var scopoAppFilters = angular.module('scopoApp.Filters', []);

scopoApp.config(function (uiSelectConfig) {
    uiSelectConfig.theme = 'selectize';
});

scopoApp.config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
    cfpLoadingBarProvider.includeSpinner = true;
    cfpLoadingBarProvider.includeBar = false;
    cfpLoadingBarProvider.latencyThreshold = 5;
}]);
