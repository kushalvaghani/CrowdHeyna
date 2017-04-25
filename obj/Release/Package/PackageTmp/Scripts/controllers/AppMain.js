var app = angular.module('app', ['ui.grid', 'ui.grid.selection', 'ui.grid.edit', 'ui.grid.rowEdit', 'ui.grid.cellNav']);

app.factory('tweetService', function ($rootScope) {

    var tweetID = 0;
    var setTweet = function (id) {
        tweetID = id;
        $rootScope.$broadcast('tweetChanged', id);
    };
    var getTweet = function () {
        return tweetID;
    }
    return {
        setTweet: setTweet,
        getTweet: getTweet
    };
});

app.controller('MainCtrl', ['$scope', '$http', '$interval', 'uiGridConstants', 'tweetService', function ($scope, $http, $interval, uiGridConstants, tweetService) {
    $scope.gridOptions = { enableRowSelection: true, enableRowHeaderSelection: false, showGridFooter: true };

    $scope.gridOptions.columnDefs = [
      { name: 'text', displayName: 'Text', cellClass: 'noborder' },
    ];

    $scope.gridOptions.multiSelect = false;
    $http.get('/api/Tweets', {
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem("accessToken")
        }
    })
      .success(function (data) {
          $scope.gridOptions.data = data;
          // $interval whilst we wait for the grid to digest the data we just gave it
          $interval(function () { $scope.gridApi.selection.selectRow($scope.gridOptions.data[0]); }, 0, 1);
      });

    $scope.gridOptions.multiSelect = false;
    $scope.gridOptions.modifierKeysToMultiSelect = false;
    $scope.gridOptions.noUnselect = true;
    $scope.gridOptions.enableHorizontalScrollbar = 0;
    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
        gridApi.selection.on.rowSelectionChanged($scope, function (row) {
            //$scope.selectedItem = row.entity;
            tweetService.setTweet(row.entity.id);
            //alert(row.entity.id);
        });
    };
}]);

app.controller('KeywordsController', ['$scope', '$http', 'tweetService', function ($scope, $http, tweetService) {

    $scope.$on('tweetChanged', function (response) {
        //alert('ss');
        $http.get('/api/Keywords?tweetid=' + tweetService.getTweet()).success(function (data) {
            $scope.data = data;
        });
        $http.get('/api/Tags').success(function (data) {
            $scope.tags = data;
        });
        $http.get('/api/Tweets?id=' + tweetService.getTweet()).success(function (data) {
            $scope.tweet = data;
        });
        $http.get('/api/KeywordUserTag?tweetid=' + tweetService.getTweet() + '&keyword=0').success(function (data) {
            $scope.keywordTagData = data;
        });
    });


    $scope.ViewSuggestions = function (keyword, tagid) {
       
        //
        $http.get('/api/UserKeywordSuggestions?keyword=' + keyword + '&id=' + tagid).success(function (data) {
            $scope.suggestions = {};
            $scope.suggestions = data;
            $("#suggestionsPanel").modal('toggle');
        });
    }

    $scope.DisplayText = function (keyword) {
        $("#" + keyword.id).css("display", "inline-table");
        $http.get('/api/KeywordUserTag?tweetid=' + tweetService.getTweet() + '&keyword=' + keyword.name).success(function (data) {
            $scope.keywordUserTagData = data;
        });
    }

    $scope.SaveKeywordText = function (keyword, tagid) {
     
        $("#" + keyword).css("display", "none");
        //$("#suggestionsPanel").modal('toggle');
        //
        var text = $("#text" + keyword+ tagid)[0].value;
        var object = [{'keywordId':keyword , 'tagId': tagid, 'suggestion':text}];
        //
        $http({
            url: 'api/UserKeywordSuggestions',
            method: "POST",
            isArray: true,
            data: JSON.stringify(object),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(function (response) {
            $("#text" + keyword)[0].value = "";
            alert('Suggestions saved, Thanks !');
        },
        function (response) { // optional
            // failed
        });

    }

    $scope.CancelKeywordText = function (keyword) {

        $("#" + keyword).css("display", "none");
        $("#suggestionsPanel").css("display", "none")
    }
}]);

app.controller('AnalyticsController', ['$scope', '$http', 'tweetService', function ($scope, $http, tweetService) {
    $scope.keywordTags = [];
    $scope.$on('tweetChanged', function (response) {
        $http.get('/api/KeywordUserTag?tweetid=' + tweetService.getTweet() + '&keyword=0').success(function (data) {
            $scope.data = data;
        });
    });
}]);

app.controller('GridController', ['$scope', '$http', '$q', '$interval', 'uiGridConstants', 'tweetService', function ($scope, $http, $q, $interval, uiGridConstants, tweetService) {
    $scope.gridOptions = {
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        selectionRowHeaderWidth: 35,
        rowHeight: 35
    };
    $scope.gridOptions.multiSelect = false;
    $scope.gridOptions.modifierKeysToMultiSelect = false;
    $scope.gridOptions.noUnselect = true;
    $scope.gridOptions.columnDefs = [
      { name: 'keyword', displayName: 'Keyword', cellClass: 'noborder', enableCellEdit: false },
      { name: 'jargon', displayName: 'Jargon', cellClass: 'noborder' },
      { name: 'abbreviation', displayName: 'Abbreviation', cellClass: 'noborder' },
      { name: 'misspelt', displayName: 'Misspelt', cellClass: 'noborder' },
      { name: 'similarity', displayName: 'Similarity', cellClass: 'noborder' }
    ];

    $scope.$on('tweetChanged', function (response) {
        $http.get('/api/Grid?tweetid=' + tweetService.getTweet(), {
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem("accessToken")
            }
        })
       .success(function (data) {
           $scope.gridOptions.data = data;
       });
    });
    $scope.changesArray = [];
    //
    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
        gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue) {
            $scope.changesArray.push(rowEntity);
        });

        gridApi.selection.on.rowSelectionChanged($scope, function (row) {

            $http.get('/api/KeywordUserTag?tweetid=' + tweetService.getTweet() + '&keyword=' + row.entity.keyword).success(function (data) {
                $scope.keywordUserTagData = data;
            });
        });
    };
    $scope.PostChanges = function () {
        $http({
            url: 'api/Grid',
            method: "POST",
            isArray: true,
            data: JSON.stringify($scope.changesArray),
            headers: {
                "Content-Type": "application/json"
            }
        })
    .then(function (response) {
        alert('Suggestions saved, Thanks !');
        $scope.changesArray = [];
        $scope.gridOptions.data = data;
        // $interval whilst we wait for the grid to digest the data we just gave it
        $interval(function () { $scope.gridApi.selection.selectRow($scope.gridOptions.data[0]); }, 0, 1);
    },
    function (response) { // optional
        // failed
    });
    };
}]);