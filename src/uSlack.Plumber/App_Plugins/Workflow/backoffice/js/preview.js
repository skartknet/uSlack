(function(){function r(e,n,t){function o(i,f){if(!n[i]){if(!e[i]){var c="function"==typeof require&&require;if(!f&&c)return c(i,!0);if(u)return u(i,!0);var a=new Error("Cannot find module '"+i+"'");throw a.code="MODULE_NOT_FOUND",a}var p=n[i]={exports:{}};e[i][0].call(p.exports,function(r){var n=e[i][1][r];return o(n||r)},p,p.exports,r,e,n,t)}return n[i].exports}for(var u="function"==typeof require&&require,i=0;i<t.length;i++)o(t[i]);return o}return r})()({1:[function(require,module,exports){
"use strict";

function _slicedToArray(arr, i) { return _arrayWithHoles(arr) || _iterableToArrayLimit(arr, i) || _nonIterableRest(); }

function _nonIterableRest() { throw new TypeError("Invalid attempt to destructure non-iterable instance"); }

function _iterableToArrayLimit(arr, i) { var _arr = []; var _n = true; var _d = false; var _e = undefined; try { for (var _i = arr[Symbol.iterator](), _s; !(_n = (_s = _i.next()).done); _n = true) { _arr.push(_s.value); if (i && _arr.length === i) break; } } catch (err) { _d = true; _e = err; } finally { try { if (!_n && _i["return"] != null) _i["return"](); } finally { if (_d) throw _e; } } return _arr; }

function _arrayWithHoles(arr) { if (Array.isArray(arr)) return arr; }

/*! plumber - v1.1.10-build380 - 2019-06-27
 * Copyright (c) 2019 Nathan Woulfe;
 * Licensed MIT
 */
angular.module('plumber.services', []);
angular.module('plumber.directives', []);
angular.module('plumber.filters', []);
angular.module('plumber', ['umbraco.security', 'umbraco.resources', 'umbraco.services', 'umbraco.packages', 'umbraco.directives', 'plumber.services', 'plumber.directives', 'plumber.filters']).config(function ($httpProvider) {
  $httpProvider.interceptors.push('plumberPreviewInterceptor');
});
var packages = angular.module('umbraco.packages', []);
/* register all interceptors 
 * 
 */

(function () {
  function interceptor($q) {
    return {
      request: function request(req) {
        if (req.url.indexOf('LocalizedText') !== -1 || req.url.indexOf('views/components') !== -1) {
          req.url = "/umbraco/" + req.url;
        }

        return req || $q.when(req);
      }
    };
  }

  angular.module('plumber').factory('plumberPreviewInterceptor', ['$q', interceptor]);
})();

(function () {
  var urlBase = '/umbraco/backoffice/api/workflow/';
  var urls = {
    tasks: urlBase + 'tasks/',
    actions: urlBase + 'actions/'
  };

  function workflowPreviewService($http, $q, umbRequestHelper, notificationsService) {
    var dialogPath = '/app_plugins/workflow/backoffice/views/dialogs/';

    var request = function request(method, url, data) {
      return umbRequestHelper.resourcePromise(method === 'GET' ? $http.get(url) : $http.post(url, data), 'Something broke');
    };
    /* workflow actions */


    var approve = function approve(instanceGuid, comment) {
      return request('POST', urls.actions + 'approve', {
        instanceGuid: instanceGuid,
        comment: comment,
        offline: true
      });
    };

    var reject = function reject(instanceGuid, comment) {
      return request('POST', urls.actions + 'reject', {
        instanceGuid: instanceGuid,
        comment: comment,
        offline: true
      });
    }; // display notification after actioning workflow task


    var notify = function notify(d) {
      if (d.status === 200) {
        notificationsService.success('SUCCESS', d.message);
      } else {
        notificationsService.error('OH SNAP', d.message);
      }

      document.querySelector('[data-element="editor-footer"]').style.display = 'none';
    };

    var service = {
      action: function action(item, type) {
        var workflowOverlay = {
          view: dialogPath + 'workflow.action.dialog.html',
          show: true,
          title: type + ' workflow process',
          subtitle: "Document: " + item.nodeName,
          approvalComment: '',
          item: item,
          submit: function submit(model) {
            // build the function name and access it via index rather than property - saves duplication
            if (type === 'Approve') {
              approve(item.instanceGuid, model.approvalComment).then(function (resp) {
                notify(resp);
              });
            } else if (type === 'Reject') {
              reject(item.instanceGuid, model.approvalComment).then(function (resp) {
                notify(resp);
              });
            }

            workflowOverlay.close();
          },
          close: function close() {
            workflowOverlay.show = false;
            workflowOverlay = null;
          }
        };
        return workflowOverlay;
      }
    };
    return service;
  }

  angular.module('plumber.services').factory('workflowPreviewService', ['$http', '$q', 'umbRequestHelper', 'notificationsService', workflowPreviewService]); // clone the workflow resource and remove a heap of unused stuff, keeping only what's needed for front-end approval

  function workflowResource($http, $q, umbRequestHelper) {
    var request = function request(method, url, data) {
      return umbRequestHelper.resourcePromise(method === 'GET' ? $http.get(url) : $http.post(url, data), 'Something broke');
    };

    return {
      getNodePendingTasks: function getNodePendingTasks(id) {
        return request('GET', urls.tasks + '/node/pending/' + id);
      },
      getAllTasksByGuid: function getAllTasksByGuid(guid) {
        return request('GET', urls.tasks + 'tasksbyguid/' + guid);
      }
    };
  } // register service


  angular.module('plumber.services').factory('plmbrWorkflowResource', ['$http', '$q', 'umbRequestHelper', workflowResource]);
})();

(function () {
  function ctrl(workflowPreviewService, workflowResource) {
    var _this = this;

    this.frameLoaded = false;
    this.invalid = false;
    this.showActionPane = false;
    /**
     * Extract cookie
     * @param {any} a = the cookie key
     */

    var getCookie = function getCookie(a) {
      var d = [],
          e = document.cookie.split(';');
      a = RegExp("^\\s*" + a + "=\\s*(.*?)\\s*$");

      for (var b = 0; b < e.length; b++) {
        var f = e[b].match(a);

        if (f) {
          d.push(f[1]);
        }
      }

      return d;
    }; // if the cookie exists, the request is invalid 


    this.invalid = getCookie('Workflow_Preview').length > 0;
    var segments = window.location.pathname.split('/');

    if (segments && segments.length === 6) {
      // domain/path/nodeid/userid/taskid/guid
      // only need the node id
      var _segments = _slicedToArray(segments, 5),
          nodeId = _segments[2];

      this.pageUrl = "/" + nodeId;

      if (!this.invalid) {
        this.action = function (actionName) {
          workflowResource.getNodePendingTasks(nodeId).then(function (resp) {
            if (resp.items) {
              _this.workflowOverlay = workflowPreviewService.action(resp.items[0], actionName);
            }
          });
        };
      }
    }
  }

  angular.module('plumber').controller('workflow.preview.controller', ['workflowPreviewService', 'plmbrWorkflowResource', ctrl]).directive('iframeIsLoaded', function () {
    return {
      restrict: 'A',
      link: function link(scope, element) {
        element.load(function () {
          var iframe = element.context.contentWindow || element.context.contentDocument;

          if (iframe && iframe.document.getElementById('umbracoPreviewBadge')) {
            iframe.document.getElementById('umbracoPreviewBadge').style.display = 'none';
          }

          if (!document.getElementById('resultFrame').contentWindow.refreshLayout) {
            scope.frameLoaded = true;
            scope.$apply();
          }
        });
      }
    };
  });
})();

(function () {
  function actionController($scope, workflowResource) {
    var _this2 = this;

    this.limit = 250;
    this.disabled = this.isFinalApproval === true ? false : true;
    this.tasksLoaded = false;

    var avatarName = function avatarName(task) {
      // don't show group if admin completed the task
      if (task.actionedByAdmin) return 'Admin'; // if not required, show the group name

      if (task.status === 4) return task.approvalGroup; // finally show either the group or the user - resubmitted tasks won't have a group, just a user

      return task.approvalGroup || task.completedBy;
    };

    var statusColor = function statusColor(status) {
      switch (status) {
        case 1:
          return 'success';
        //approved

        case 2:
          return 'warning';
        //rejected

        case 3:
          return 'gray';
        //pending

        case 4:
          return 'info';
        //not required

        case 5:
          return 'danger';
        //cancelled

        case 6:
          return 'danger';
        //error

        default:
          return 'gray';
        //resubmitted
      }
    };

    var whodunnit = function whodunnit(task) {
      // if rejected or incomplete, use the group name
      if (task.status === 4 || !task.completedBy) return task.approvalGroup; // if actioned by an admin, show

      if (task.actionedByAdmin) return task.completedBy + " as Admin"; // if approved, show the user and group name

      if (task.approvalGroup) return task.completedBy + " for " + task.approvalGroup; // otherwise, just show the user name - resubmitted tasks don't have a group

      return task.completedBy;
    };
    /**
     * If the instance has status === error, the error message is on the author comment
     * wrapped in square brackets. This extracts it.
     * @returns {string} c
     */


    this.extractErrorFromComment = function () {
      var c = $scope.model.item.comment;
      return c.substring(c.indexOf('[') + 1, c.length - 1);
    };
    /**
     * Fetch all tasks for the current workflow instance
     * Then build a UI-ready object
     * TODO => review this. Tasks exist on $scope.model.item, but need current/total step values
     */


    workflowResource.getAllTasksByGuid($scope.model.item.instanceGuid).then(function (resp) {
      _this2.tasksLoaded = true; // current step should only count approved tasks - maybe rejected/resubmitted into

      _this2.currentStep = resp.currentStep;
      _this2.totalSteps = resp.totalSteps; // there may be multiple tasks for a given step, due to rejection/resubmission
      // modify the tasks object to nest tasks

      _this2.tasks = [];
      resp.items.forEach(function (t) {
        // push some extra UI strings onto each task
        t.avatarName = avatarName(t);
        t.statusColor = statusColor(t.status);
        t.whodunnit = whodunnit(t);

        if (!_this2.tasks[t.currentStep]) {
          _this2.tasks[t.currentStep] = [];
        }

        _this2.tasks[t.currentStep].push(t);
      });
    });
  }

  angular.module('plumber').controller('Workflow.Action.Controller', ['$scope', 'plmbrWorkflowResource', actionController]);
})();

(function () {
  /**
   * Set the icon for the given task, based on the stauts
   * @param { } task 
   * @returns { string } 
   */
  function iconName() {
    return function (task) {
      var response = ''; //rejected

      if (task.status === 2) {
        response = 'delete';
      } // resubmitted or approved


      if (task.status === 7 || task.status === 1) {
        response = 'check';
      } // pending


      if (task.status === 3) {
        response = 'record';
      } // not required


      if (task.status === 4) {
        response = 'next-media';
      } // error


      if (task.status === 6) {
        response = 'thumbs-down';
      } // not required


      if (task.status === 5) {
        response = 'stop';
      }

      return response;
    };
  }

  angular.module('plumber.filters').filter('iconName', iconName);
})();

(function () {
  var template = "\n                <div>\n                    <p ng-bind=\"intro\"></p>\n                    <label for=\"comments\">\n                        {{ labelText }} <span ng-bind=\"info\"></span>\n                    </label>\n                    <textarea no-dirty-check id=\"comments\" ng-model=\"comment\" ng-change=\"limitChars()\" umb-auto-focus></textarea>\n                </div>";

  function comments() {
    var directive = {
      restrict: 'AEC',
      scope: {
        intro: '=',
        labelText: '=',
        comment: '=',
        limit: '=',
        isFinalApproval: '=',
        disabled: '='
      },
      template: template,
      link: function link(scope) {
        scope.limitChars = function () {
          var limit = scope.limit;

          if (scope.comment.length > limit) {
            scope.info = "(Comment max length exceeded - limit is " + limit + " characters.)";
            scope.comment = scope.comment.substr(0, limit);
          } else {
            scope.info = "(" + (limit - scope.comment.length) + " characters remaining.)";
          }

          if (!scope.isFinalApproval) {
            scope.disabled = scope.comment.length === 0;
          }
        };
      }
    };
    return directive;
  }

  angular.module('plumber.directives').directive('wfComments', comments);
})();

},{}]},{},[1]);
