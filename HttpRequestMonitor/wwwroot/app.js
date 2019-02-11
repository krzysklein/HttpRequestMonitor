(function () {
    const app = angular.module('app', []);
    const httpRequestBrokerHub = new signalR.HubConnectionBuilder().withUrl('/httpRequestBrokerHub').build();

    app.controller('MainController', function ($scope) {
        $scope.title = 'HTTP Request Monitor';
        $scope.requests = [];

        $scope.clear = function () {
            $scope.requests = [];
        };

        httpRequestBrokerHub.on('HttpRequestReceived', function (url, method, headers, body) {
            console.log('HttpRequestReceived', url, method, headers, body);
            $scope.$apply(function () {
                $scope.requests.push({
                    url: url,
                    method: method,
                    headers: headers,
                    body: body
                });
            });
        });
        httpRequestBrokerHub.start().then(function () {
            console.log('SignalR has started');
        }).catch(function (err) {
            return console.error(err.toString());
        });
    });


})();
