(function(){function r(e,n,t){function o(i,f){if(!n[i]){if(!e[i]){var c="function"==typeof require&&require;if(!f&&c)return c(i,!0);if(u)return u(i,!0);var a=new Error("Cannot find module '"+i+"'");throw a.code="MODULE_NOT_FOUND",a}var p=n[i]={exports:{}};e[i][0].call(p.exports,function(r){var n=e[i][1][r];return o(n||r)},p,p.exports,r,e,n,t)}return n[i].exports}for(var u="function"==typeof require&&require,i=0;i<t.length;i++)o(t[i]);return o}return r})()({1:[function(require,module,exports){
"use strict";

function _slicedToArray(arr, i) { return _arrayWithHoles(arr) || _iterableToArrayLimit(arr, i) || _nonIterableRest(); }

function _nonIterableRest() { throw new TypeError("Invalid attempt to destructure non-iterable instance"); }

function _iterableToArrayLimit(arr, i) { var _arr = []; var _n = true; var _d = false; var _e = undefined; try { for (var _i = arr[Symbol.iterator](), _s; !(_n = (_s = _i.next()).done); _n = true) { _arr.push(_s.value); if (i && _arr.length === i) break; } } catch (err) { _d = true; _e = err; } finally { try { if (!_n && _i["return"] != null) _i["return"](); } finally { if (_d) throw _e; } } return _arr; }

function _arrayWithHoles(arr) { if (Array.isArray(arr)) return arr; }

function _typeof(obj) { if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") { _typeof = function (_typeof2) { function _typeof(_x) { return _typeof2.apply(this, arguments); } _typeof.toString = function () { return _typeof2.toString(); }; return _typeof; }(function (obj) { return typeof obj === "undefined" ? "undefined" : _typeof(obj); }); } else { _typeof = function (_typeof3) { function _typeof(_x2) { return _typeof3.apply(this, arguments); } _typeof.toString = function () { return _typeof3.toString(); }; return _typeof; }(function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj === "undefined" ? "undefined" : _typeof(obj); }); } return _typeof(obj); }

/*! plumber - v1.1.10-build380 - 2019-06-27
 * Copyright (c) 2019 Nathan Woulfe;
 * Licensed MIT
 */
(function () {
  angular.module('plumber.directives', []);
  angular.module('plumber.filters', []);
  angular.module('plumber.services', []);
  angular.module('plumber', ['plumber.directives', 'plumber.filters', 'plumber.services']);
  angular.module('umbraco').requires.push('plumber');
})();

(function () {
  'use strict';

  function dashboardController(workflowResource, tourService) {
    var _this = this;

    var storeKey = 'plumberUpdatePrompt';
    var msPerDay = 1000 * 60 * 60 * 24;
    var now = moment();
    this.tours = [{
      alias: 'plmbrGroups',
      name: 'Approval groups'
    }, {
      alias: 'plmbrConfig',
      name: 'Configuring workflows'
    }, {
      alias: 'plmbrSettings',
      name: 'Global settings'
    }];
    /**
     * Returns an array of 0s, length equal to the selected range
     */

    var defaultData = function defaultData() {
      return Array(_this.range).fill([]).map(function () {
        return 0;
      });
    };

    var lineChart = function lineChart(items) {
      var series = [],
          seriesNames = [],
          s,
          o,
          isTask = _this.type === 'Task';
      var d = new Date();
      d.setDate(d.getDate() - _this.range);
      var then = Date.UTC(d.getFullYear(), d.getMonth(), d.getDate());
      var created = {
        name: 'Total (cumulative)',
        type: 'spline',
        data: defaultData(),
        pointStart: then,
        pointInterval: msPerDay,
        className: 'wf-highcharts-color-total',
        lineWidth: 4,
        marker: {
          enabled: false,
          fillColor: null,
          lineColor: null
        }
      };
      items.forEach(function (v) {
        var statusName = isTask ? v.statusName : v.status; // bit messy, but need to modify some returned name values
        // type 1|3 status 7 -> rejected
        // type 2 status 7 -> resubmit
        // status 3 -> pending
        // status 1 -> approved
        // status 4 -> not required

        if (v.type !== 2 && v.status === 7) {
          statusName = v.statusName = 'Rejected';
        } else if (v.type === 2 && v.status === 7) {
          statusName = v.statusName = 'Resubmitted';
        }

        if (statusName !== 'Pending Approval') {
          if (seriesNames.indexOf(statusName) === -1) {
            o = {
              name: statusName,
              type: 'column',
              data: defaultData(),
              pointStart: then,
              pointInterval: msPerDay
            };
            series.push(o);
            seriesNames.push(statusName);
          }

          s = series.filter(function (ss) {
            return ss.name === statusName;
          })[0];
          s.data[_this.range - now.diff(moment(v.completedDate), 'days')] += 1;
          created.data[_this.range - now.diff(moment(v.createdDate), 'days')] += 1;

          if (statusName === 'Approved') {
            _this.totalApproved += 1;
            s.className = 'wf-highcharts-color-approved';
          } else if (statusName === 'Rejected') {
            _this.totalRejected += 1;
            s.className = 'wf-highcharts-color-rejected';
          } else if (statusName === 'Resubmitted') {
            _this.totalResubmitted += 1;
            s.className = 'wf-highcharts-color-resubmitted';
          } else if (statusName === 'Not Required') {
            _this.totalNotRequired += 1;
            s.className = 'wf-highcharts-color-notreq';
          } else {
            _this.totalCancelled += 1;
            s.className = 'wf-highcharts-color-cancelled';
          }
        } else {
          var index = _this.range - now.diff(moment(v.createdDate), 'days');
          created.data[index < 0 ? 0 : index] += 1;
          _this.totalPending += 1;
        }
      });
      created.data.forEach(function (v, i) {
        if (i > 0) {
          created.data[i] += created.data[i - 1];
        }
      });
      series.push(created);
      _this.series = series.sort(function (a, b) {
        return a.name > b.name;
      });
      _this.title = "Workflow " + _this.type.toLowerCase() + " activity";
      _this.loaded = true;
    };

    var getForRange = function getForRange() {
      if (_this.range > 0) {
        _this.totalApproved = 0;
        _this.totalCancelled = 0;
        _this.totalPending = 0;
        _this.totalRejected = 0;
        _this.totalResubmitted = 0;
        _this.totalNotRequired = 0;
        _this.loaded = false;
        _this.totalApproved = _this.totalCancelled = _this.totalPending = _this.totalRejected = 0;
        workflowResource[_this.type === 'Task' ? 'getAllTasksForRange' : 'getAllInstancesForRange'](_this.range).then(function (resp) {
          lineChart(resp.items);
        });
      }
    }; // check the current installed version against the remote on GitHub, only if the 
    // alert has never been dismissed, or was dismissed more than 7 days ago


    var pesterDate = localStorage.getItem(storeKey);

    if (!pesterDate || moment(new Date(pesterDate)).isBefore(now)) {
      workflowResource.getVersion().then(function (resp) {
        if ((typeof resp === "undefined" ? "undefined" : _typeof(resp)) === 'object') {
          _this.version = resp;
        }
      });
    }

    var updateAlertHidden = function updateAlertHidden() {
      localStorage.setItem(storeKey, now.add(7, 'days'));
    }; // try fetch a tour, to determine if tours are enabled


    tourService.getTourByAlias('plmbrConfig').then(function (resp) {
      if (resp) {
        _this.showTours = true;
      }
    }); // start selected tour

    var launchTour = function launchTour(tourAlias) {
      tourService.getTourByAlias(tourAlias).then(function (resp) {
        tourService.startTour(resp);
      });
    };

    var getActivity = function getActivity(filter, friendly) {
      workflowResource.setActivityFilter({
        type: _this.type,
        filter: filter,
        range: _this.range,
        friendly: friendly
      });
      window.location = Umbraco.Sys.ServerVariables.umbracoSettings.umbracoPath + '/#/workflow/workflow/history/info';
    }; // kick it off with a four-week span


    angular.extend(this, {
      range: 28,
      type: 'Task',
      loaded: false,
      totalApproved: 0,
      totalCancelled: 0,
      totalPending: 0,
      totalRejected: 0,
      totalResubmitted: 0,
      totalNotRequired: 0,
      getForRange: getForRange,
      updateAlertHidden: updateAlertHidden,
      launchTour: launchTour,
      getActivity: getActivity
    });
    getForRange();
  }

  angular.module('plumber').controller('Workflow.AdminDashboard.Controller', ['plmbrWorkflowResource', 'tourService', dashboardController]);
})();

(function () {
  'use strict';
  /**
   * Docs are fetched from a parsed markdown file on GitHub - it needs to be parsed into a JSON object to use the healthcheck-style UI
   * Keeping the raw file as markdown makes for easier editing, but does add some processing overhead on the client
   * @param {any} workflowResource
   */

  function dashboardController($timeout, workflowResource) {
    var _this2 = this;

    this.viewState = 'list';
    this.selectedDoc = {};
    /**
     * Allow links in docs to open other sections, based on simple matching on the hash and doc name
     */

    var openDocFromDoc = function openDocFromDoc(e) {
      e.preventDefault(); // on click, get the anchor, find the correct section and switch to it

      var target = _this2.docs.filter(function (v) {
        var name = v.name.toLowerCase().replace(' ', '-');
        return name.indexOf(e.target.hash.substring(1)) === 0;
      })[0];

      if (target) {
        _this2.openDoc(target);
      }
    };

    var bindListeners = function bindListeners() {
      $timeout(function () {
        var elms = document.querySelectorAll('.umb-healthcheck-group__details-check-description a');

        if (elms.length) {
          for (var i = 0; i < elms.length; i += 1) {
            elms[i].addEventListener('click', function (e) {
              openDocFromDoc(e);
            });
          }
        }
      });
    };
    /**
     * 
     * @param {any} node
     * @param {any} index
     * @param {any} elements
     */


    var getContentForHeading = function getContentForHeading(node, index, elements) {
      var html = '';

      for (var i = index + 1; i < elements.length; i += 1) {
        if (elements[i].tagName !== 'H3') {
          html += elements[i].outerHTML;
        } else {
          break;
        }
      }

      return html;
    };
    /**
     * 
     * @param {any} docs
     */


    var parseDocs = function parseDocs(docs) {
      var parser = new DOMParser();
      var article = angular.element(parser.parseFromString(docs, 'text/html')).find('article');
      var elements = article.children();
      var json = [];
      angular.forEach(elements, function (v, i) {
        if (v.tagName === 'H3') {
          json.push({
            name: v.innerText,
            content: getContentForHeading(v, i, elements)
          });
        }
      });
      _this2.docs = json;
      _this2.loaded = true;
    };
    /**
     * 
     * @param {any} doc
     */


    this.openDoc = function (doc) {
      _this2.selectedDoc = doc;
      _this2.viewState = 'details'; // this will only be the current open doc

      bindListeners();
    };
    /**
     * 
     * @param {any} state
     */


    this.setViewState = function (state) {
      _this2.viewState = state;
    };

    workflowResource.getDocs().then(function (docs) {
      if (docs === 'Documentation unavailable') {
        _this2.noDocs = docs;
        _this2.loaded = true;
      } else {
        parseDocs(docs);
      }
    });
  }

  angular.module('plumber').controller('Workflow.DocsDashboard.Controller', ['$timeout', 'plmbrWorkflowResource', dashboardController]);
})();

(function () {
  'use strict';

  function dashboardController($rootScope, workflowGroupsResource) {
    var _this3 = this;

    this.name = 'Approval groups';
    this.loading = true;
    this.items = [];

    this.init = function () {
      workflowGroupsResource.get().then(function (resp) {
        _this3.loading = false;
        _this3.items = resp;
      });
    };

    this.getEmail = function (users) {
      return users.map(function (v) {
        return v.user.email;
      }).join(';');
    };

    $rootScope.$on('refreshGroupsDash', function () {
      _this3.init();
    });
    this.init();
  }

  angular.module('plumber').controller('Workflow.Groups.Dashboard.Controller', ['$rootScope', 'plmbrGroupsResource', dashboardController]);
})();

(function () {
  'use strict';

  function importexportController(workflowResource, notificationsService) {
    var _this4 = this;

    this.doImport = function () {
      workflowResource.doImport(_this4.importData).then(function (resp) {
        if (resp) {
          notificationsService.success('SUCCESS', 'Plumber config imported successfully');
        } else {
          notificationsService.error('ERROR', 'Plumber config import failed');
        }
      });
    };

    this.doExport = function () {
      workflowResource.doExport().then(function (resp) {
        _this4.exportData = JSON.stringify(resp);
      });
    };
  }

  angular.module('plumber').controller('Workflow.ImportExport.Controller', ['plmbrWorkflowResource', 'notificationsService', importexportController]);
})();

(function () {
  'use strict';

  function logController(workflowResource) {
    var _this5 = this;

    var refresh = function refresh() {
      workflowResource.getLog().then(function (resp) {
        _this5.html = resp;
      });
      workflowResource.getLogDates().then(function (resp) {
        // resp is an array of log dates, where [0] is 'txt', for the current date as the source file is undated
        _this5.datePickerConfig.minDate = resp.length > 1 ? moment(resp[1]) : moment();
      });
    };

    var datePickerChange = function datePickerChange(event) {
      // handle change for a valid date - fetch corresponding log file if date is ok
      if (event.date && event.date.isValid() && event.oldDate.isValid()) {
        var date = event.date.format('YYYY-MM-DD');
        workflowResource.getLog(date === moment().format('YYYY-MM-DD') ? '' : date).then(function (resp) {
          _this5.html = resp;
        });
      }
    };

    var datePickerError = function datePickerError() {// handle error
    };

    angular.extend(this, {
      simple: true,
      filter: 'all',
      datePickerConfig: {
        defaultDate: moment(),
        maxDate: moment(),
        pickDate: true,
        pickTime: false,
        format: 'D MMM YYYY',
        icons: {
          time: 'icon-time',
          date: 'icon-calendar',
          up: 'icon-chevron-up',
          down: 'icon-chevron-down'
        }
      },
      refresh: refresh,
      datePickerChange: datePickerChange,
      datePickerError: datePickerError
    });
    refresh();
  }

  angular.module('plumber').controller('Workflow.Log.Controller', ['plmbrWorkflowResource', logController]);
})();

(function () {
  'use strict';

  function dashboardController($scope, $rootScope, $routeParams, workflowResource, authResource, notificationsService, plumberHub) {
    var _this6 = this;

    var notify = null;

    var getPending = function getPending() {
      // api call for tasks assigned to the current user
      workflowResource.getApprovalsForUser(_this6.currentUser.id, _this6.taskPagination.perPage, _this6.taskPagination.pageNumber).then(function (resp) {
        _this6.tasks = resp.items;
        _this6.taskPagination.pageNumber = resp.page;
        _this6.taskPagination.totalPages = resp.total / resp.count;
        _this6.loaded[0] = true;
      }, function (err) {
        notify(err);
      });
    };

    var getSubmissions = function getSubmissions() {
      // api call for tasks created by the current user
      workflowResource.getSubmissionsForUser(_this6.currentUser.id, _this6.submissionPagination.perPage, _this6.submissionPagination.pageNumber).then(function (resp) {
        _this6.submissions = resp.items;
        _this6.submissionPagination.pageNumber = resp.page;
        _this6.submissionPagination.totalPages = resp.total / resp.count;
        _this6.loaded[1] = true;
      }, function (err) {
        notify(err);
      });
    };

    var getAdmin = function getAdmin() {
      // if the current user is in an admin group, display all active tasks
      if (_this6.adminUser) {
        workflowResource.getPendingTasks(_this6.adminPagination.perPage, _this6.adminPagination.pageNumber).then(function (resp) {
          _this6.activeTasks = resp.items;
          _this6.adminPagination.pageNumber = resp.page;
          _this6.adminPagination.totalPages = resp.totalPages;
          _this6.loaded[2] = true;
        }, function (err) {
          notify(err);
        });
      }
    };

    var goToPage = function goToPage(i) {
      _this6.pagination.pageNumber = i;
    };

    var init = function init() {
      getPending();
      getSubmissions();
      getAdmin();
    }; // dash needs notification of when to refresh, as the action is in a deeper scope


    $rootScope.$on('refreshWorkflowDash', function () {
      init();
    }); // display notification after actioning workflow task

    notify = function notify(d) {
      if (d.status === 200) {
        notificationsService.success('SUCCESS!', d.message);
        init();
      } else {
        notificationsService.error('OH SNAP!', d.message);
      }
    };

    var addTask = function addTask(task) {
      if (task && task.permissions) {
        var permission = task.permissions.filter(function (p) {
          return p.groupId === task.approvalGroupId;
        }); // these are independent and can all be true

        if (permission.length && permission[0].userGroup.usersSummary.indexOf("|" + _this6.currentUser.id + "|") !== -1) {
          _this6.tasks.push(task);
        }

        if (task.requestedById === _this6.currentUser.id) {
          _this6.submissions.push(task);
        }

        if (_this6.adminUser) {
          _this6.activeTasks.push(task);
        }
      }
    };

    var removeTask = function removeTask(task) {
      var taskId = task.taskId;

      _this6.tasks.splice(_this6.tasks.findIndex(function (i) {
        return i.taskId === taskId;
      }), 1);

      _this6.submissions.splice(_this6.tasks.findIndex(function (i) {
        return i.taskId === taskId;
      }), 1);

      if (_this6.adminUser) {
        _this6.activeTasks.splice(_this6.tasks.findIndex(function (i) {
          return i.taskId === taskId;
        }), 1);
      }
    }; // subscribe to signalr magick


    plumberHub.initHub(function (hub) {
      hub.on('workflowStarted', function (e) {
        addTask(e);
      });
      hub.on('taskCancelled', function (e) {
        addTask(e);
      });
      hub.on('taskApproved', function (e) {
        // add the newest task
        addTask(e[0]); // remove the previous tasks

        removeTask(e.splice(1));
      });
      hub.on('taskRejected', function (e) {
        // add the newest task
        addTask(e[0]); // remove the previous tasks

        removeTask(e.splice(1));
      });
      hub.start();
    }); // expose some bits

    angular.extend(this, {
      tasks: [],
      submissions: [],
      activeTasks: [],
      loaded: [false, false, false],
      goToPage: goToPage,
      taskPagination: {
        pageNumber: 1,
        totalPages: 0,
        perPage: 5,
        goToPage: function goToPage(i) {
          _this6.taskPagination.pageNumber = i;
          getPending();
        }
      },
      submissionPagination: {
        pageNumber: 1,
        totalPages: 0,
        perPage: 5,
        goToPage: function goToPage(i) {
          _this6.submissionPagination.pageNumber = i;
          getSubmissions();
        }
      },
      adminPagination: {
        pageNumber: 1,
        totalPages: 0,
        perPage: 10,
        goToPage: function goToPage(i) {
          _this6.adminPagination.pageNumber = i;
          getAdmin();
        }
      }
    }); // check the root node has permissions defined

    workflowResource.workflowConfigured().then(function (resp) {
      if (Object.keys(resp).length) {
        notificationsService.add({
          view: Umbraco.Sys.ServerVariables.workflow.overlayPath + "/workflow.notconfigured.html",
          args: {
            nodes: resp
          }
        });
      }
    }); // kick it all off

    authResource.getCurrentUser().then(function (user) {
      _this6.currentUser = user;
      _this6.adminUser = user.allowedSections.indexOf('workflow') !== -1;
      init();
    });
  } // register controller 


  angular.module('plumber').controller('Workflow.UserDashboard.Controller', ['$scope', '$rootScope', '$routeParams', 'plmbrWorkflowResource', 'authResource', 'notificationsService', 'plumberHub', dashboardController]);
})();

(function () {
  'use strict';

  function addController($scope, workflowGroupsResource, navigationService, notificationsService, treeService) {
    var _this7 = this;

    $scope.$watch('name', function () {
      _this7.failed = false;
    });

    this.add = function (name) {
      workflowGroupsResource.add(name).then(function (resp) {
        if (resp.status === 200) {
          if (resp.success === true) {
            treeService.loadNodeChildren({
              node: $scope.$parent.currentNode.parent(),
              section: 'workflow'
            }).then(function () {
              window.location = Umbraco.Sys.ServerVariables.umbracoSettings.umbracoPath + "/#/workflow/workflow/edit-group/" + resp.id;
              navigationService.hideNavigation();
            });
            notificationsService.success('SUCCESS', resp.msg);
          } else {
            _this7.failed = true;
            _this7.msg = resp.msg;
          }
        } else {
          notificationsService.error('ERROR', resp.msg);
        }
      }, function (err) {
        notificationsService.error('ERROR', err);
      });
    };

    this.cancelAdd = function () {
      navigationService.hideNavigation();
    };
  }

  angular.module('plumber').controller('Workflow.Groups.Add.Controller', ['$scope', 'plmbrGroupsResource', 'navigationService', 'notificationsService', 'treeService', addController]);
})();

(function () {
  'use strict';

  function deleteController($scope, $rootScope, workflowGroupsResource, navigationService, treeService, notificationsService) {
    this.delete = function (id) {
      workflowGroupsResource.delete(id).then(function (resp) {
        treeService.loadNodeChildren({
          node: $scope.$parent.currentNode.parent(),
          section: 'workflow'
        }).then(function () {
          navigationService.hideNavigation();
          notificationsService.success('SUCCESS', resp);
          $rootScope.$emit('refreshGroupsDash');
        });
      });
    };

    this.cancelDelete = function () {
      navigationService.hideNavigation();
    };
  }

  angular.module('plumber').controller('Workflow.Groups.Delete.Controller', ['$scope', '$rootScope', 'plmbrGroupsResource', 'navigationService', 'treeService', 'notificationsService', deleteController]);
})();

(function () {
  function editController($scope, $routeParams, $location, workflowGroupsResource, workflowResource, notificationsService, contentResource, navigationService) {
    var _this8 = this;

    this.view = '';

    var getContentTypes = function getContentTypes() {
      _this8.nodePermissions = _this8.group.permissions.filter(function (v) {
        return v.nodeId;
      });
      _this8.docPermissions = _this8.group.permissions.filter(function (v) {
        return v.contentTypeId;
      });

      if (_this8.nodePermissions.length) {
        contentResource.getByIds(_this8.nodePermissions.map(function (v) {
          return v.nodeId;
        })).then(function (resp) {
          resp.forEach(function (v) {
            _this8.nodePermissions.forEach(function (p) {
              if (p.nodeId === v.id) {
                p.icon = v.icon;
                p.path = v.path;
                p.name = v.name + ' - stage ' + (p.permission + 1);
              }
            });
          });
        });
      }

      if (_this8.docPermissions.length) {
        workflowResource.getContentTypes().then(function (resp) {
          resp.forEach(function (v) {
            _this8.docPermissions.forEach(function (p) {
              if (p.contentTypeId === v.id) {
                p.icon = v.icon;
                p.path = v.path;
                p.name = v.name + ' - stage ' + (p.permission + 1);
              }
            });
          });
        });
      }
    }; // history tab


    var getHistory = function getHistory() {
      workflowResource.getAllTasksForGroup($routeParams.id, _this8.pagination.perPage, _this8.pagination.pageNumber).then(function (resp) {
        _this8.tasks = resp.items;
        _this8.pagination.pageNumber = resp.page;
        _this8.pagination.totalPages = resp.totalPages;
        _this8.tasksLoaded = true;
        _this8.view = 'group';
      });
    };

    this.editDocTypePermission = function () {
      $location.path('/workflow/workflow/settings/info');
    };

    this.perPage = function () {
      return [2, 5, 10, 20, 50];
    }; // todo -> Would be sweet to open the config dialog from here, rather than just navigating to the node...


    this.editContentPermission = function (id) {
      navigationService.changeSection('content');
      $location.path("/content/content/edit/" + id);
    };
    /**
     * Remove a user from the group
     * @param {any} id
     */


    this.remove = function (id) {
      var index;

      _this8.group.users.forEach(function (u, i) {
        if (u.userId === id) {
          index = i;
        }
      });

      _this8.group.users.splice(index, 1);
    };
    /**
     * Open the picker to add a new user to the group
     */


    this.openUserPicker = function () {
      _this8.userPicker = {
        view: '../app_plugins/workflow/backoffice/views/dialogs/workflow.userpicker.overlay.html',
        selection: _this8.group.users,
        show: true,
        submit: function submit(model) {
          _this8.userPicker.show = false;
          _this8.userPicker = null;
          _this8.group.users = []; // this is a bit clunky - save only needs the userId and groupId, the user object is discarded
          // it's used in the view though

          model.selection.forEach(function (u) {
            _this8.group.users.push({
              userId: u.userId || u.id,
              groupId: _this8.group.groupId,
              user: u.user || u
            });
          });
        },
        close: function close() {
          _this8.userPicker.show = false;
          _this8.userPicker = null;
        }
      };
    };
    /**
     * Save the group and show appropriate notifications
     */


    this.save = function () {
      workflowGroupsResource.save(_this8.group).then(function (resp) {
        if (resp.status === 200) {
          notificationsService.success('SUCCESS', resp.msg);
          $scope.approvalGroupForm.$setPristine();
        } else {
          notificationsService.error('ERROR', resp.msg);
        }
      }, function (err) {
        notificationsService.error('ERROR', err);
      });
    };
    /**
     * Fetch the group by the given id, or create an empty model if the id is -1 (ie a new group - id doesn't exist until saving)
     */


    var init = function init() {
      _this8.loaded = false;

      if ($routeParams.id !== '-1') {
        workflowGroupsResource.get($routeParams.id).then(function (resp) {
          _this8.group = resp;
          _this8.name = $routeParams.id !== '-1' ? 'Edit ' : "Create " + resp.name;

          if (_this8.group.permissions) {
            getContentTypes();
          }

          _this8.loaded = true;
        });
      } else {
        _this8.group = {
          groupId: -1,
          name: '',
          description: '',
          alias: '',
          groupEmail: '',
          users: [],
          usersSummary: ''
        };
        _this8.loaded = true;
      }
    }; // declare scoped variables


    this.tabs = [{
      id: 0,
      label: 'Group detail',
      alias: 'tab0',
      active: true
    }, {
      id: 1,
      label: 'Activity history',
      alias: 'tab1',
      active: false
    }];
    this.pagination = {
      pageNumber: 1,
      totalPages: 0,
      perPage: 10,
      goToPage: function goToPage(i) {
        _this8.pagination.pageNumber = i;
        getHistory();
      }
    }; // get the data

    init();
    getHistory();
  }

  angular.module('plumber').controller('Workflow.Groups.Edit.Controller', ['$scope', '$routeParams', '$location', 'plmbrGroupsResource', 'plmbrWorkflowResource', 'notificationsService', 'contentResource', 'navigationService', editController]);
})();

(function () {
  function actionController($scope, workflowResource) {
    var _this9 = this;

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
      _this9.tasksLoaded = true; // current step should only count approved tasks - maybe rejected/resubmitted into

      _this9.currentStep = resp.currentStep;
      _this9.totalSteps = resp.totalSteps; // there may be multiple tasks for a given step, due to rejection/resubmission
      // modify the tasks object to nest tasks

      _this9.tasks = [];
      resp.items.forEach(function (t) {
        // push some extra UI strings onto each task
        t.avatarName = avatarName(t);
        t.statusColor = statusColor(t.status);
        t.whodunnit = whodunnit(t);

        if (!_this9.tasks[t.currentStep]) {
          _this9.tasks[t.currentStep] = [];
        }

        _this9.tasks[t.currentStep].push(t);
      });
    });
  }

  angular.module('plumber').controller('Workflow.Action.Controller', ['$scope', 'plmbrWorkflowResource', actionController]);
})();

(function () {
  'use strict'; // create controller 

  function cancelController($scope) {
    $scope.model.comment = '';
    this.limit = 250;
    this.intro = 'This operation will cancel the workflow on this document and notify the workflow participants. Are you sure?';
    this.disabled = $scope.model.isFinalApproval === true ? false : true;
    $scope.$watch('model.comment', function (newVal) {
      $scope.model.hideSubmitButton = !newVal || newVal.length === 0;
    });
  } // register controller 


  angular.module('plumber').controller('Workflow.Cancel.Controller', ['$scope', cancelController]);
})();

(function () {
  'use strict'; // create controller 

  function configController($scope, $rootScope, $q, workflowGroupsResource, workflowResource, notificationsService, contentResource, navigationService) {
    var _this10 = this;

    var nodeId = $scope.dialogOptions.currentNode ? $scope.dialogOptions.currentNode.id : undefined;
    var nodeIdInt = nodeId ? parseInt(nodeId, 10) : undefined;
    this.inherited = [];
    this.approvalPath = [];
    this.contentTypeApprovalPath = [];
    this.sortOptions = {
      axis: 'y',
      cursor: 'move',
      handle: '.sort-handle',
      stop: function stop() {}
    };
    var node = void 0;
    var settings = void 0;
    /**
     * Fetch the groups and content type data
     */

    var init = function init() {
      _this10.contentTypeAlias = node.contentTypeAlias;
      _this10.contentTypeName = node.contentTypeName;
      var nodePerms = workflowResource.checkNodePermissions(_this10.groups, nodeIdInt, _this10.contentTypeAlias);
      _this10.approvalPath = nodePerms.approvalPath;
      _this10.contentTypeApprovalPath = nodePerms.contentTypeApprovalPath;
      _this10.inherited = workflowResource.checkAncestorPermissions(node.path, _this10.groups);

      if (!_this10.excludeNode) {
        _this10.activeType = _this10.approvalPath.length ? 'content' : _this10.contentTypeApprovalPath.length ? 'type' : _this10.inherited.length ? 'inherited' : null;
      }
    };

    if (!nodeId) {
      navigationService.hideDialog();
      notificationsService.error('ERROR', 'No active content node');
    }
    /**
     * Process the approvalPath object, then save it
     */


    this.save = function () {
      var response = {};
      response[nodeIdInt] = []; // convert the approvalPath array into something resembling the expected model
      // Dictionary<int, List<UserGroupPermissionsPoco>

      _this10.approvalPath.forEach(function (v, i) {
        response[nodeIdInt].push({
          nodeId: nodeId,
          permission: i,
          groupId: v.groupId
        });
      });

      workflowResource.saveConfig(response).then(function () {
        navigationService.hideNavigation();
        notificationsService.success('SUCCESS', 'Workflow configuration updated');
        $rootScope.$broadcast('configUpdated');
        init();
      }, function (err) {
        navigationService.hideNavigation();
        notificationsService.error('ERROR', err);
      });
    };
    /**
     * Adds a stage to the approval flow
     */


    this.add = function () {
      _this10.selectedApprovalGroup.permissions.push({
        nodeId: nodeId,
        permission: _this10.approvalPath.length,
        groupId: _this10.selectedApprovalGroup.groupId
      });

      _this10.approvalPath.push(_this10.selectedApprovalGroup);
    };
    /**
     * Removes a stage from the approval flow
     * @param {any} $event
     * @param {any} index
     */


    this.remove = function ($event, index) {
      $event.stopPropagation();
      $event.target.classList.add('disabled');

      _this10.approvalPath.splice(index, 1);

      _this10.approvalPath.forEach(function (v, i) {
        v.permissions.forEach(function (p) {
          if (p.nodeId === nodeIdInt) {
            p.permission = i;
          }
        });
      });
    }; // it all starts here


    var promises = [contentResource.getById(nodeId), workflowResource.getSettings(), workflowGroupsResource.get()];
    $q.all(promises).then(function (resp) {
      var _resp = _slicedToArray(resp, 3);

      node = _resp[0];
      settings = _resp[1];
      _this10.groups = _resp[2];
      _this10.excludeNode = workflowResource.checkExclusion(settings.excludeNodes, nodeId);
      init();
    });
  } // register controller 


  angular.module('plumber').controller('Workflow.Config.Controller', ['$scope', '$rootScope', '$q', 'plmbrGroupsResource', 'plmbrWorkflowResource', 'notificationsService', 'contentResource', 'navigationService', configController]);
})();

(function () {
  'use strict'; // create controller 

  function contentTypeFlowController($scope) {
    var _this11 = this;

    this.properties = [];
    this.approvalPath = [];
    this.conditions = [];

    var updateSortOrder = function updateSortOrder() {};

    if ($scope.model.type) {
      // if it's an edit, path is on the type, so shift it for convience later
      this.approvalPath = $scope.model.type.approvalPath;
      $scope.model.type.propertyGroups.forEach(function (pg) {
        pg.propertyTypes.forEach(function (pt) {
          _this11.properties.push(pt);
        });
      }); // approvalPath will also hold the conditional keys, if any exist
      // use these to build the conditions array

      this.approvalPath.forEach(function (step) {
        var steps = step.permissions.filter(function (p) {
          return p.contentTypeId === $scope.model.type.id;
        });

        if (steps) {
          steps.forEach(function (s) {
            if (s.condition) {
              s.condition.forEach(function (c) {
                _this11.conditions.push({
                  groupName: step.name,
                  groupId: step.groupId,
                  condition: c
                });
              });
            }
          });
        }
      });
    } else {
      this.isAdd = true;
    }
    /**
     * 
     * @returns {} 
     */


    this.addCondition = function () {
      _this11.conditions.push({});
    };
    /**
     * 
     * @param {} $event 
     * @param {} $index 
     * @returns {} 
     */


    this.removeCondition = function ($event, index, condition, groupId) {
      $event.stopPropagation();

      _this11.conditions.splice(index, 1);

      _this11.approvalPath.forEach(function (step) {
        if (step.groupId === groupId) {
          step.permissions.forEach(function (p) {
            if (p.contentTypeId === $scope.model.type.id) {
              p.condition.splice(p.condition.indexOf(condition), 1);
            }
          });
        }
      });
    };
    /**
     * 
     * @param {} groupId 
     * @param {} condition 
     * @returns {} 
     */


    this.setCondition = function (groupId, condition, oldValue) {
      _this11.approvalPath.forEach(function (group) {
        if (group.groupId === groupId) {
          group.permissions.forEach(function (p) {
            if (p.contentTypeId === $scope.model.type.id) {
              if (p.condition) {
                // if oldvalue exists, replace it since this is a change to an existing condition
                // oldvalue won't exist if it's a new condition
                var oldIndex = p.condition.indexOf(oldValue);

                if (oldIndex !== -1) {
                  p.condition[oldIndex] = condition;
                } else {
                  p.condition.push(condition);
                }
              } else {
                p.condition = [condition];
              }
            }
          });
        }
      });
    };
    /**
     * 
     */


    this.add = function () {
      // when adding a new config, type will not exist.
      if (!$scope.model.type) {
        $scope.model.type = {};
      } // the group assigned to the stage also needs a permission object created - this isn't stored anywhere, just used for UI stuff


      _this11.selectedApprovalGroup.permissions.push({
        contentTypeId: $scope.model.type.id,
        permission: _this11.approvalPath.length,
        groupId: _this11.selectedApprovalGroup.groupId
      });

      _this11.approvalPath.push(_this11.selectedApprovalGroup);

      $scope.model.type.approvalPath = _this11.approvalPath;
    };
    /**
     * 
     * @param {any} $event
     * @param {any} index
     */


    this.remove = function ($event, index, groupId) {
      $event.stopPropagation();

      _this11.approvalPath.splice(index, 1);

      $scope.model.type.approvalPath = _this11.approvalPath; // also remove any conditions - can't do in the existing method as params are different.

      if (_this11.conditions.length > 0) {
        _this11.conditions = _this11.conditions.filter(function (c) {
          return c.groupId !== groupId;
        });
      }
    };

    this.multiNames = function () {
      var names = $scope.model.typesToAdd.map(function (m) {
        return m.name;
      });
      var resp = '';

      if (names.length === 1) {
        resp = names[0];
      } else if (names.length === 2) {
        resp = names.join(' and ');
      } else if (names.length > 2) {
        resp = names.slice(0, -1).join(', ') + ', and ' + names.slice(-1);
      } // if it's a multi  and an add, populate type if there is only one selected
      // doing it here so that it updates when/if the types are added/removed from model.multi


      if ($scope.model.typesToAdd.length === 1) {
        $scope.model.type = $scope.model.typesToAdd[0];
      }

      return resp;
    };

    this.sortOptions = {
      axis: 'y',
      cursor: 'move',
      handle: '.sort-handle',
      stop: function stop() {
        updateSortOrder();
      }
    };
  } // register controller 


  angular.module('plumber').controller('Workflow.ContentTypeFlow.Controller', ['$scope', contentTypeFlowController]);
})();

(function () {
  'use strict';

  function controller($scope, $rootScope, $q, $window, userService, workflowResource, workflowGroupsResource, workflowActionsService, contentEditingHelper, editorState, $routeParams, notificationsService, plumberHub) {
    var _this12 = this;

    this.active = false;
    this.excludeNode = false;
    this.buttonGroupState = 'init';
    var workflowConfigured = false;
    var dirty = false;
    var user = void 0;
    var settings = void 0;
    var groups = void 0;
    var dashboardClick = editorState.current === null;
    var defaultButtons = contentEditingHelper.configureContentEditorButtons({
      create: $routeParams.create,
      content: $scope.content,
      methods: {
        saveAndPublish: $scope.saveAndPublish,
        sendToPublish: $scope.sendToPublish,
        save: $scope.save,
        unPublish: $scope.unPublish
      }
    });
    var defaultUnpublish = void 0;

    if (defaultButtons.subButtons) {
      defaultUnpublish = defaultButtons.subButtons.filter(function (x) {
        return x.alias === 'unpublish';
      })[0];
    }

    var saveAndPublish = defaultButtons.defaultButton && defaultButtons.defaultButton.labelKey === 'buttons_saveAndPublish';
    var buttons = {
      approveButton: {
        labelKey: 'workflow_approveButtonLong',
        handler: function handler(item) {
          _this12.workflowOverlay = workflowActionsService.action(item, 'Approve', dashboardClick);
        }
      },
      cancelButton: {
        labelKey: 'workflow_cancelButtonLong',
        cssClass: 'danger',
        handler: function handler(item) {
          _this12.workflowOverlay = workflowActionsService.cancel(item, dashboardClick);
        }
      },
      rejectButton: {
        labelKey: 'workflow_rejectButton',
        cssClass: 'warning',
        handler: function handler(item) {
          _this12.workflowOverlay = workflowActionsService.action(item, 'Reject', dashboardClick);
        }
      },
      resubmitButton: {
        labelKey: 'workflow_resubmitButton',
        handler: function handler(item) {
          _this12.workflowOverlay = workflowActionsService.action(item, 'Resubmit', dashboardClick);
        }
      },
      detailButton: {
        labelKey: 'workflow_detailButton',
        handler: function handler(item) {
          _this12.workflowOverlay = workflowActionsService.detail(item);
        }
      },
      saveButton: {
        labelKey: 'workflow_saveButton',
        cssClass: 'success',
        handler: $scope.save
      },
      publishButton: {
        labelKey: 'workflow_publishButton',
        cssClass: 'success',
        handler: function handler() {
          var that = _this12; // Perform a Save first, to ensure we catch scenario where the user hasn't been presented
          // with a Save button due to issues with Umbraco's dirty-checking

          $scope.save().then(function (d) {
            // There's no way to know if the Save succeeded from here, as Umbraco returns 200 when
            // the Saving event is canceled, and the promise only rejects for 500 errors.  For now,
            // we'll determine success by looking for any "Success" notifications in the response.
            var saveSucceeded = d.notifications.some(function (notification) {
              return notification.type === 3;
            });

            if (saveSucceeded) {
              that.workflowOverlay = workflowActionsService.initiate(editorState.current.name, editorState.current.id, true);
            } else {
              notificationsService.error('Workflow: Unable to request publish, saving the content failed');
            }
          }, function () {
            notificationsService.error('Workflow', 'Unable to request publish, saving the content failed');
          });
        }
      },
      unpublishButton: {
        labelKey: 'workflow_unpublishButton',
        cssClass: 'warning',
        handler: function handler() {
          _this12.workflowOverlay = workflowActionsService.initiate(editorState.current.name, editorState.current.id, false);
        }
      }
    };
    /**
     * any user with access to the workflow section will be able to action workflows ie cancel outside their group membership
     * @param {any} task
     */

    var checkUserAccess = function checkUserAccess(task) {
      _this12.task = task || _this12.task;
      _this12.canAction = false;
      _this12.isAdmin = user.allowedSections.indexOf('workflow') !== -1;
      var currentTaskUsers = _this12.task.permissions[_this12.task.currentStep].userGroup.usersSummary;

      if (currentTaskUsers.indexOf("|" + user.id + "|") !== -1) {
        _this12.canAction = true;
      }

      if (_this12.active) {
        _this12.buttonGroup = {};

        if (dirty && (_this12.userCanEdit || _this12.canAction && !settings.lockIfActive)) {
          _this12.buttonGroup.defaultButton = buttons.saveButton;
        } // primary button is approve when the user is in the approving group and task is not rejected
        else if (_this12.canAction && !_this12.rejected) {
            _this12.buttonGroup.defaultButton = buttons.approveButton;
          } else if (_this12.userCanEdit) {
            // rejected tasks show the resubmit, only when the user is the original author
            _this12.buttonGroup.defaultButton = buttons.resubmitButton;
          } else {
            // all other cases see the detail button
            _this12.buttonGroup.defaultButton = buttons.detailButton;
          }

        _this12.buttonGroup.subButtons = []; // if the default button isn't detail, it should be first in the sub button set

        if (_this12.buttonGroup.defaultButton !== buttons.detailButton) {
          _this12.buttonGroup.subButtons.push(buttons.detailButton);
        } // if the user is in the approving group, and the task is not rejected, add reject to sub buttons


        if (_this12.canAction && !_this12.rejected) {
          _this12.buttonGroup.subButtons.push(buttons.rejectButton);
        } // if the user is admin, the change author or in the approving group for a non-rejected task, add the cancel button


        if (_this12.isAdmin || _this12.userCanEdit || _this12.isChangeAuthor || _this12.canAction && !_this12.rejected) {
          _this12.buttonGroup.subButtons.push(buttons.cancelButton);
        }
      }
    };
    /**
     * Manages the default states for the buttons - updates when no active task, or when the content form is dirtied
     */


    var setButtons = function setButtons() {
      // default button will be null when the current user has browse-only permission
      _this12.buttonGroup = {};

      if (workflowConfigured && defaultButtons.defaultButton !== null) {
        var subButtons = saveAndPublish ? [buttons.unpublishButton, defaultButtons.defaultButton, buttons.saveButton] : [buttons.unpublishButton, buttons.saveButton]; // insert the default unpublish button into the subbutton array

        if (saveAndPublish && defaultUnpublish) {
          subButtons.splice(1, 0, defaultUnpublish);
        } // if the content is dirty, show save. otherwise show request approval


        _this12.buttonGroup = {
          defaultButton: dirty ? buttons.saveButton : buttons.publishButton,
          subButtons: dirty ? saveAndPublish ? [defaultButtons.defaultButton] : [] : subButtons
        };
      } else {
        if (defaultButtons.defaultButton !== null && !_this12.active) {
          _this12.buttonGroup = defaultButtons;
        }
      } // if a task is active, the default buttons should be updated to match the current user's access/role in the workflow


      if (_this12.active) {
        checkUserAccess();
      }
    };
    /**
     * 
     */


    var getPendingTasks = function getPendingTasks() {
      workflowResource.getNodePendingTasks(editorState.current.id).then(function (resp) {
        if (resp.items && resp.items.length) {
          _this12.active = true; // if the workflow status is rejected, the original author should be able to edit and resubmit

          var currentTask = resp.items.reduce(function (prev, current) {
            return prev.taskId > current.taskId ? prev : current;
          });
          _this12.rejected = currentTask.cssStatus === 'rejected'; // if the task has been rejected and the current user requested the change, let them edit

          _this12.isChangeAuthor = currentTask.requestedById === user.id;
          _this12.userCanEdit = _this12.rejected && _this12.isChangeAuthor;
          checkUserAccess(currentTask);
        } else {
          _this12.active = false;
          setButtons();
        }
      }, function () {});
    };

    var getNodeTasks = function getNodeTasks() {
      // only refresh if viewing a content node
      if (editorState.current && !editorState.current.trashed) {
        // check if the node is included in the workflow model
        // groups has been fetched already
        var nodePerms = workflowResource.checkNodePermissions(groups, editorState.current.id, editorState.current.contentTypeAlias);
        var ancestorPerms = workflowResource.checkAncestorPermissions(editorState.current.path, groups);

        if ((nodePerms.approvalPath.length || nodePerms.contentTypeApprovalPath.length || ancestorPerms.length) && !_this12.excludeNode) {
          workflowConfigured = true;
          getPendingTasks();
        } else {
          workflowConfigured = false;
          _this12.buttonGroup = defaultButtons;
        }
      }
    }; // use this to ensure changes are saved when submitting for publish
    // event is broadcast from the buttons directive, which watches the content form


    $rootScope.$on('contentFormDirty', function (event, data) {
      dirty = data;
      setButtons();
    }); // ensures dash/buttons refresh

    $rootScope.$on('workflowActioned', function () {
      getNodeTasks();
    });
    $rootScope.$on('configUpdated', function () {
      getNodeTasks();
    }); // subscribe to signalr magick for button state
    // events are raised in ActionController - doesn't matter what they return, only care that they are raised
    // as it indicates a change of state for the button
    //const hubEvent = id => {
    //    if (!dashboardClick && id === editorState.current.id) {
    //        getNodeTasks();
    //    }
    //};
    //plumberHub.initHub(hub => {
    //    hub.on('workflowStarted', data => {
    //        hubEvent(data.nodeId);
    //    });
    //    hub.on('taskCancelled', data => {
    //        hubEvent(data.nodeId);
    //    });
    //    hub.on('taskApproved', data => {
    //        hubEvent(data.nodeId);
    //    });
    //    hub.on('taskRejected', data => {
    //        hubEvent(data.nodeId);
    //    });
    //    hub.start();
    //});
    // preview should not save, if the content is in a workflow

    this.preview = function (content) {
      // Chromes popup blocker will kick in if a window is opened 
      // outwith the initial scoped request. This trick will fix that.
      var previewWindow = $window.open("preview/?id=" + content.id, 'umbpreview'); // Build the correct path so both /#/ and #/ work.

      var redirect = Umbraco.Sys.ServerVariables.umbracoSettings.umbracoPath + '/preview/?id=' + content.id;
      previewWindow.location.href = redirect;
    }; // it all starts here


    var promises = [userService.getCurrentUser(), workflowResource.getSettings(), workflowGroupsResource.get()];
    $q.all(promises).then(function (resp) {
      var _resp2 = _slicedToArray(resp, 3);

      user = _resp2[0];
      settings = _resp2[1];
      groups = _resp2[2];
      _this12.excludeNode = workflowResource.checkExclusion(settings.excludeNodes, editorState.current.path);
      getNodeTasks();
    });
  } // register controller 


  angular.module('plumber').controller('Workflow.DrawerButtons.Controller', ['$scope', '$rootScope', '$q', '$window', 'userService', 'plmbrWorkflowResource', 'plmbrGroupsResource', 'plmbrActionsService', 'contentEditingHelper', 'editorState', '$routeParams', 'notificationsService', 'plumberHub', controller]);
})();

(function () {
  function historyController($scope, workflowResource) {
    var _this13 = this;

    this.activityFilter = workflowResource.getActivityFilter();

    this.perPage = function () {
      return [2, 5, 10, 20, 50];
    };

    this.name = 'Workflow history';
    this.view = '';
    this.pagination = {
      pageNumber: 1,
      totalPages: 0,
      perPage: 10,
      goToPage: function goToPage(i) {
        _this13.pagination.pageNumber = i;

        if (_this13.activityFilter) {
          _this13.getActivity();
        } else if (_this13.node !== undefined) {
          _this13.auditNode();
        } else {
          _this13.getAllInstances();
        }
      }
    };
    var width = $scope.dialogOptions ? $scope.dialogOptions.currentAction.metaData.width : undefined;
    var node = $scope.dialogOptions ? $scope.dialogOptions.currentNode : undefined;

    var setPaging = function setPaging(resp) {
      _this13.items = resp.items;
      _this13.pagination.pageNumber = resp.page;
      _this13.pagination.totalPages = resp.totalPages;
      _this13.loading = false;
    };

    if (width) {
      angular.element('#dialog').css('width', width);
    }

    this.selectNode = function () {
      _this13.overlay = {
        view: 'contentpicker',
        show: true,
        submit: function submit(model) {
          if (model.selection) {
            _this13.auditNode(model.selection[0]);
          } else {
            $scope.items = [];
          }

          _this13.overlay.close();
        },
        close: function close() {
          _this13.overlay.show = false;
          _this13.overlay = null;
        }
      };
    };
    /**
     * 
     */


    this.getAllInstances = function () {
      _this13.loading = true; // when switching, set state, reset paging and clear node data

      if (_this13.view !== 'instance') {
        _this13.view = 'instance';
        _this13.pagination.pageNumber = 1;
        _this13.node = undefined;
      }

      workflowResource.getAllInstances(_this13.pagination.perPage, _this13.pagination.pageNumber).then(function (resp) {
        setPaging(resp);
        _this13.instancesLoaded = true;
      });
    };
    /**
     * 
     * @param {any} data
     */


    this.auditNode = function (data) {
      _this13.loading = true; // when switching from instance to node, reset paging, toggle state and store node

      if (_this13.view !== 'node') {
        _this13.pagination.pageNumber = 1;
        _this13.view = 'node';
      }

      _this13.node = data || _this13.node;
      workflowResource.getAllInstancesForNode(_this13.node.id, _this13.pagination.perPage, _this13.pagination.pageNumber).then(function (resp) {
        setPaging(resp);
        _this13.nodeInstancesLoaded = true;
      });
    };
    /**
     * 
     */


    var getActivity = function getActivity() {
      if (_this13.view.indexOf('activity') === -1) {
        _this13.pagination.pageNumber = 1;
        _this13.node = undefined;
        _this13.view = "activity-" + _this13.activityFilter.type.toLowerCase();
      }

      workflowResource[_this13.activityFilter.type === 'Task' ? 'getFilteredTasksForRange' : 'getFilteredInstancesForRange'](_this13.activityFilter.range, _this13.activityFilter.filter, _this13.pagination.perPage, _this13.pagination.pageNumber).then(function (resp) {
        setPaging(resp);
        _this13.activityLoaded = true;
      });
    }; // go get the data


    if (this.activityFilter) {
      getActivity();
    } else if (node) {
      this.auditNode(node);
    } else {
      this.getAllInstances();
    }
  }

  angular.module('plumber').controller('Workflow.History.Controller', ['$scope', 'plmbrWorkflowResource', historyController]);
})();

(function () {
  function notConfiguredController(notificationsService) {
    this.nodeNames = notificationsService.current[0].args.nodes;

    this.discard = function (not) {
      notificationsService.remove(not);
    };
  } // register controller 


  angular.module('plumber').controller('Workflow.NotConfigured.Controller', ['notificationsService', notConfiguredController]);
})();

(function () {
  'use strict';

  function settingsController($scope, $q, workflowResource, notificationsService, workflowGroupsResource, contentResource) {
    var _this14 = this;

    var promises = [workflowResource.getSettings(), workflowResource.getContentTypes(), workflowGroupsResource.get()];
    var overlayBase = Umbraco.Sys.ServerVariables.workflow.overlayPath;
    this.excludeNodesModel = {
      view: 'contentpicker',
      editor: 'Umbraco.MultiNodeTreePicker2',
      alias: 'excludeNodesPicker',
      config: {
        multiPicker: '1',
        maxNumber: null,
        minNumber: null,
        idType: 'id',
        showEditButton: '0',
        showOpenButton: '0',
        showPathOnHover: '0',
        startNode: {
          type: 'content'
        }
      }
    };
    this.name = 'Workflow settings';
    this.email = '';
    this.defaultApprover = '';
    this.settings = {
      email: '',
      defaultApprover: ''
    };
    $q.all(promises).then(function (resp) {
      var _resp3 = _slicedToArray(resp, 3);

      _this14.settings = _resp3[0];
      _this14.docTypes = _resp3[1];
      _this14.groups = _resp3[2];

      if (_this14.settings.excludeNodes) {
        _this14.excludeNodesModel.value = _this14.settings.excludeNodes; // this feels super hacky - fetch nodes and push into the content picker
        // there's a watch in the picker controller, but it's not seeing changes to the value

        var picker = document.querySelector('#exclude-nodes-picker ng-form');

        if (picker) {
          var s = angular.element(picker).scope();

          _this14.settings.excludeNodes.split(',').forEach(function (id) {
            contentResource.getById(id).then(function (entity) {
              s.add(entity);
            });
          });
        }
      }

      _this14.flowTypes = [{
        i: 0,
        v: 'Explicit'
      }, {
        i: 1,
        v: 'Implicit'
      }];
      _this14.flowType = _this14.flowTypes[_this14.settings.flowType];

      if (_this14.settings.defaultApprover) {
        _this14.defaultApprover = _this14.groups.filter(function (g) {
          return parseInt(g.groupId, 10) === parseInt(_this14.settings.defaultApprover, 10);
        })[0];
      }

      _this14.groups.forEach(function (g) {
        g.permissions.forEach(function (p) {
          if (p.contentTypeId > 0) {
            if (p.condition) {
              p.condition = p.condition.split(',');
            }

            _this14.docTypes.forEach(function (dt) {
              if (dt.id === p.contentTypeId) {
                if (!dt.approvalPath) {
                  dt.approvalPath = [];
                }

                dt.approvalPath[p.permission] = g;
              }
            });
          }
        });
      });
    });

    this.save = function () {
      var permissions = {};
      _this14.settings.defaultApprover = _this14.defaultApprover ? _this14.defaultApprover.groupId : '';
      _this14.settings.flowType = _this14.flowType.i;

      if (_this14.excludeNodesModel.value) {
        _this14.settings.excludeNodes = _this14.excludeNodesModel.value;
      } // convert the approval path group collection into a set of permissions objects for saving
      // means we're holding extra data, but makes it easier to manipulate as it's less abstract


      _this14.docTypes.forEach(function (dt, i) {
        if (dt.approvalPath && dt.approvalPath.length) {
          permissions[i] = [];
          dt.approvalPath.forEach(function (path, ii) {
            var permissionObject = {
              contentTypeId: dt.id,
              permission: ii,
              groupId: path.groupId
            }; // filter the permissions for cases where the contenttype id and permission index match the current loop iteration

            var permissionsForStep = path.permissions.filter(function (perm) {
              return perm.contentTypeId === dt.id && perm.permission === ii;
            })[0]; // if a permission exists, push it onto the current permission object

            if (permissionsForStep && permissionsForStep.condition) {
              permissionObject.condition = permissionsForStep.condition.join(',');
            }

            permissions[i].push(permissionObject);
          });
        }
      });

      var p = [workflowResource.saveDocTypeConfig(permissions), workflowResource.saveSettings(_this14.settings)];
      $q.all(p).then(function () {
        notificationsService.success('SUCCESS!', 'Settings updated');
      }, function (err) {
        notificationsService.error('OH SNAP!', err);
      });
    };
    /**
     * Removes the approval path for the group, which will remove it from config on save
     * @param {any} type
     */


    this.removeDocTypeFlow = function (type) {
      delete type.approvalPath;
    };

    this.editDocTypeFlow = function (type) {
      _this14.overlay = {
        view: overlayBase + "/workflow.contenttypeflow.overlay.html",
        show: true,
        type: type,
        groups: _this14.groups,
        types: _this14.docTypes.filter(function (v) {
          return !v.approvalPath;
        }),
        title: (type ? 'Edit' : 'Add') + " content type approval flow",
        submit: function submit(model) {
          // map the updated approval path back onto the doctypes collection 
          if (model.type.approvalPath.length) {
            // multi has a value when adding - can add more than one
            var ids = model.typesToAdd ? model.typesToAdd.map(function (t) {
              return t.id;
            }) : [model.type.id];

            _this14.docTypes.forEach(function (v) {
              if (ids.indexOf(v.id) !== -1) {
                v.approvalPath = model.type.approvalPath;
              }
            });
          }

          _this14.overlay.close();
        },
        close: function close() {
          _this14.overlay.show = false;
          _this14.overlay = null;
        }
      };
    };

    this.hasApprovalPath = function (d) {
      return d.approvalPath !== undefined;
    };
  }

  angular.module('plumber').controller('Workflow.Settings.Controller', ['$scope', '$q', 'plmbrWorkflowResource', 'notificationsService', 'plmbrGroupsResource', 'contentResource', settingsController]);
})();

(function () {
  'use strict';

  var submitController = function submitController($scope) {
    $scope.$watch('model.comment', function (newVal) {
      $scope.model.hideSubmitButton = !newVal || newVal.length === 0;
    });
  };

  angular.module('plumber').controller('Workflow.Submit.Controller', ['$scope', submitController]);
})(); // this is almost identical to the Umbraco default, only the id property on the user object is changed to userId


(function () {
  'use strict';

  function userPickerController($scope, usersResource, localizationService) {
    var _this15 = this;

    this.users = [];
    this.loading = false;
    this.usersOptions = {}; //////////

    var preSelect = function preSelect(selection, users) {
      selection.forEach(function (selected) {
        users.forEach(function (user) {
          if (selected.userId === user.id) {
            user.selected = true;
          }
        });
      });
    };

    var getUsers = function getUsers() {
      _this15.loading = true; // Get users

      usersResource.getPagedResults(_this15.usersOptions).then(function (users) {
        _this15.users = users.items;
        _this15.usersOptions.pageNumber = users.pageNumber;
        _this15.usersOptions.pageSize = users.pageSize;
        _this15.usersOptions.totalItems = users.totalItems;
        _this15.usersOptions.totalPages = users.totalPages;
        preSelect($scope.model.selection, _this15.users);
        _this15.loading = false;
      });
    };

    var search = _.debounce(function () {
      $scope.$apply(function () {
        getUsers();
      });
    }, 500);

    var onInit = function onInit() {
      _this15.loading = true; // set default title

      if (!$scope.model.title) {
        $scope.model.title = localizationService.localize('defaultdialogs_selectUsers');
      } // make sure we can push to something


      if (!$scope.model.selection) {
        $scope.model.selection = [];
      } // get users


      getUsers();
    };

    this.searchUsers = function () {
      search();
    };

    this.changePageNumber = function (pageNumber) {
      _this15.usersOptions.pageNumber = pageNumber;
      getUsers();
    };

    this.selectUser = function (user) {
      if (!user.selected) {
        user.selected = true;
        $scope.model.selection.push(user);
      } else {
        $scope.model.selection.forEach(function (selectedUser, index) {
          if (selectedUser.userId === user.id) {
            user.selected = false;
            $scope.model.selection.splice(index, 1);
          }
        });
      }
    };

    onInit();
  }

  angular.module('plumber').controller('Workflow.UserPicker.Controller', ['$scope', 'usersResource', 'localizationService', userPickerController]);
})();

(function () {
  'use strict';

  function buttonGroupDirective($rootScope, angularHelper, editorState, workflowActionsService) {
    var directive = {
      restrict: 'E',
      replace: true,
      templateUrl: '../app_plugins/workflow/backoffice/views/partials/workflowButtonGroup.html',
      require: '^form',
      scope: {
        defaultButton: '=',
        subButtons: '=',
        state: '=?',
        item: '=',
        direction: '@?',
        float: '@?',
        drawer: '@?'
      },
      link: function link(scope, elm, attr, contentForm) {
        scope.detail = function (item) {
          scope.workflowOverlay = workflowActionsService.detail(item);
        };

        scope.state = 'init'; // can watch the content form state in the directive, then broadcast the state change

        scope.$watch(function () {
          return contentForm.$dirty;
        }, function (newVal) {
          $rootScope.$broadcast('contentFormDirty', newVal);
        });
        $rootScope.$on('buttonStateChanged', function (event, data) {
          if (scope.item && scope.item.nodeId === data.id || editorState.current && editorState.current.id === data.id) {
            scope.state = data.state; // button might be in a dashboard, so need to check for content form before resetting form state

            if (editorState.current && contentForm) {
              contentForm.$setPristine();
            }
          }
        });
      }
    };
    return directive;
  }

  angular.module('plumber.directives').directive('workflowButtonGroup', ['$rootScope', 'angularHelper', 'editorState', 'plmbrActionsService', buttonGroupDirective]);
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

(function () {
  function instances(workflowActionsService) {
    var directive = {
      restrict: 'E',
      scope: {
        items: '=',
        loaded: '=',
        view: '='
      },
      templateUrl: '../app_plugins/workflow/backoffice/views/partials/workflowInstanceTemplate.html',
      link: function link(scope) {
        scope.detail = function (item) {
          scope.instanceOverlay = workflowActionsService.detail(item);
        };

        scope.$watch('view', function () {
          scope.showProgress = scope.view.indexOf('activity') === -1 && scope.view !== 'group';
          scope.showName = scope.view === 'instance' || scope.view.indexOf('activity') === 0 || scope.view === 'group';
        });
      }
    };
    return directive;
  }

  angular.module('plumber.directives').directive('wfInstance', ['plmbrActionsService', instances]);
})();

(function () {
  'use strict';

  function lineChart() {
    var directive = {
      restrict: 'E',
      template: '<div class="chart-container"><div></div></div>',
      scope: {
        series: '=',
        ready: '='
      },
      link: function link(scope, element) {
        var el = element[0].querySelector('.chart-container div');
        scope.$watch('ready', function (newVal) {
          if (newVal === true) {
            var options = {
              credits: {
                enabled: false
              },
              title: {
                text: null
              },
              legend: {
                itemStyle: {
                  fontSize: '15px'
                }
              },
              tooltip: {
                shared: true,
                formatter: function formatter() {
                  var r = this.points.filter(function (p) {
                    return p.y > 0;
                  }).length > 0;

                  if (!r) {
                    return false;
                  }

                  var s = "<span>" + new Date(this.x).toDateString() + "</span><br />";
                  this.points.forEach(function (p) {
                    if (p.y > 0) {
                      s += "<span class=\"wf-highcharts-color-" + p.series.name.toLowerCase().replace(' ', '-') + "\">\u25CF</span> " + p.series.name + ": <b>" + p.y + "</b><br/>";
                    }
                  });
                  return s;
                }
              },
              series: scope.series,
              xAxis: {
                type: 'datetime',
                dateTimeLabelFormats: {
                  day: '%b %e'
                }
              },
              yAxis: {
                allowDecimals: false,
                minTickInterval: 1,
                min: 0,
                type: 'logarithmic',
                title: {
                  text: null
                }
              }
            };
            Highcharts.chart(el, options);
          }
        });
      }
    };
    return directive;
  }

  angular.module('plumber.directives').directive('wfLineChart', lineChart);
})();

(function () {
  // directives here are used to change the icon on nodes in a list view
  // fetches the status for all nodes in the current view
  // sets a class on the list view row if an active workflow exists
  // raises an event once that is complete
  // in the table row directive, the event triggers adding a class to the table row which changes the icon and title attribute
  function tableRow() {
    var directive = {
      restrict: 'C',
      link: function link(scope, element) {
        scope.$on('listViewStatus', function () {
          if (scope.item && scope.item.activeWorkflow) {
            element[0].classList.add('active-workflow');
            element[0].childNodes.forEach(function (c) {
              if (c.classList && c.classList.contains('umb-table-cell')) {
                c.setAttribute('title', 'Workflow active');
              }
            });
          }
        });
      }
    };
    return directive;
  }

  angular.module('plumber.directives').directive('umbTableRow', tableRow);

  function listview(workflowResource) {
    var directive = {
      restrict: 'C',
      link: function link(scope) {
        var setIcons = function setIcons(nodes) {
          scope.listViewResultSet.items.forEach(function (v) {
            v.activeWorkflow = nodes[v.id] && nodes[v.id] === true;
          });
        };

        scope.$watch(function () {
          return scope.listViewResultSet.items;
        }, function (a, b) {
          if (a && a.length && a !== b) {
            scope.items = a;
            scope.ids = scope.items.map(function (i) {
              return i.id;
            });
            workflowResource.getStatus(scope.ids.join(',')).then(function (resp) {
              setIcons(resp.nodes);
              scope.$broadcast('listViewStatus');
            });
          }
        });
      }
    };
    return directive;
  }

  angular.module('plumber.directives').directive('umbListview', ['plmbrWorkflowResource', listview]);
})();

(function () {
  var template = "\n        <div class=\"progress-step {{ css[0] }}\" ng-class=\"{ 'last-of-type' : lastOfType, 'no-gradient' : noGradient }\" ng-style=\"{ 'width' : width }\">\n            <span class=\"marker\">\n                <i class=\"icon-\"></i>\n            </span>\n            <span class=\"tooltip\">\n                <span class=\"tooltip-{{ css[0] }}\" ng-bind=\"css[1]\"></span>\n                {{ task.userGroup.name }}\n            </span>\n        </div>";

  function progressStep() {
    var directive = {
      restrict: 'E',
      replace: true,
      scope: {
        task: '=',
        status: '=',
        total: '=',
        current: '='
      },
      template: template,
      link: function link(scope) {
        scope.$watch('task', function () {
          scope.width = 100 / scope.total + "%";
          scope.css = scope.current > scope.task.permission ? ['done', 'Approved by'] : scope.current === scope.task.permission && scope.status === 'rejected' ? ['current', 'Rejected by'] : scope.current === scope.task.permission ? ['current', 'Pending'] : ['pending', 'Pending'];
          scope.lastOfType = scope.task.permission + 1 === scope.current;
          scope.noGradient = scope.task.permission < scope.current - 1;
        }, true);
      }
    };
    return directive;
  }

  angular.module('plumber.directives').directive('wfProgressStep', progressStep);
})();

(function () {
  function tasks($location, workflowActionsService) {
    var directive = {
      restrict: 'AEC',
      scope: {
        items: '=',
        type: '=',
        loaded: '='
      },
      templateUrl: '../app_plugins/workflow/backoffice/views/partials/workflowTasksTemplate.html',
      controller: function controller($scope) {
        // type = 0, 1
        // 0 -> full button set
        // 1 -> cancel, edit - this is reversed if the task is rejected
        // 2 -> no buttons
        $scope.detail = function (item) {
          console.log(item);
          $scope.$parent.vm.workflowOverlay = workflowActionsService.detail(item);
        };

        var buttons = {
          approveButton: {
            labelKey: 'workflow_approveButton',
            handler: function handler(item) {
              $scope.$parent.vm.workflowOverlay = workflowActionsService.action(item, 'Approve', true);
            }
          },
          editButton: {
            labelKey: 'workflow_editButton',
            handler: function handler(item) {
              $location.path("/content/content/edit/" + item.nodeId);
            }
          },
          cancelButton: {
            labelKey: 'workflow_cancelButton',
            cssClass: 'danger',
            handler: function handler(item) {
              $scope.$parent.vm.workflowOverlay = workflowActionsService.cancel(item, true);
            }
          },
          rejectButton: {
            labelKey: 'workflow_rejectButton',
            cssClass: 'warning',
            handler: function handler(item) {
              $scope.$parent.vm.workflowOverlay = workflowActionsService.action(item, 'Reject', true);
            }
          }
        };
        var subButtons = [[buttons.editButton, buttons.rejectButton, buttons.cancelButton], [buttons.editButton], [buttons.cancelButton]];

        if ($scope.type !== 2) {
          $scope.buttonGroup = {
            defaultButton: $scope.type === 0 ? buttons.approveButton : buttons.cancelButton,
            subButtons: subButtons[$scope.type]
          };
        } else {
          $scope.noActions = true;
        } // when the items arrive, if a task was rejected
        // flip the order of the cancel and edit buttons


        $scope.$watch('items', function (newVal) {
          if (newVal && newVal.length && $scope.type === 0) {
            $scope.items.forEach(function (i) {
              if (i.cssStatus === 'rejected') {
                $scope.buttonGroup.defaultButton = buttons.editButton;
                $scope.buttonGroup.subButtons = [buttons.cancelButton];
              }
            });
          }
        });
      }
    };
    return directive;
  }

  angular.module('plumber.directives').directive('wfTasks', ['$location', 'plmbrActionsService', tasks]);
})();
/* register all interceptors 
 * 
 */


(function () {
  'use strict';

  angular.module('plumber').config(function ($httpProvider) {
    $httpProvider.interceptors.push('drawerButtonsInterceptor');
  });
})();

(function () {
  // replace the editor buttons with Plumber's version
  function interceptor($q) {
    return {
      request: function request(req) {
        if (req.url.toLowerCase().indexOf('footer-content-right') !== -1) {
          if (location.hash.indexOf('content') !== -1) {
            req.url = '../app_plugins/workflow/backoffice/views/partials/workflowEditorFooterContentRight.html';
          }
        }

        return req || $q.when(req);
      }
    };
  }

  angular.module('plumber').factory('drawerButtonsInterceptor', ['$q', interceptor]);
})();

(function () {
  'use strict';

  function workflowActionsService($rootScope, workflowResource, notificationsService) {
    var overlayPath = Umbraco.Sys.ServerVariables.workflow.overlayPath; // UI feedback for button directive

    var _buttonState = function _buttonState(state, id) {
      $rootScope.$emit('buttonStateChanged', {
        state: state,
        id: id
      });
    }; // display notification after actioning workflow task


    var notify = function notify(d, fromDash, id) {
      // display any notifications generated by the workflow process
      if (d.hasOwnProperty('notifications') && d.notifications.length > 0) {
        d.notifications.map(function (n) {
          notificationsService.showNotification(n);
        });
      }

      $rootScope.$emit('workflowActioned');

      if (fromDash) {
        $rootScope.$emit('refreshWorkflowDash');
      }

      if (d.status === 200 && !d.isUmbracoOperationError) {
        // UmbracoOperationFailedExceptions return 200's, so need to check both cases for success
        notificationsService.success('SUCCESS', d.message);

        _buttonState('success', id);
      } else {
        if (d.isUmbracoOperationError) {
          notificationsService.error('FAILURE', d.message);
        } else {
          notificationsService.error('OH SNAP', d.data.ExceptionMessage);
        }

        _buttonState('error', id);
      }
    };

    var service = {
      action: function action(item, type, fromDash) {
        var workflowOverlay = {
          view: overlayPath + "/workflow.action.dialog.html",
          show: true,
          title: type + ' workflow process',
          subtitle: "Document: " + item.nodeName,
          approvalComment: '',
          item: item,
          submit: function submit(model) {
            _buttonState('busy', item.nodeId); // build the function name and access it via index rather than property - saves duplication


            var functionName = type.toLowerCase() + 'WorkflowTask';
            workflowResource[functionName](item.instanceGuid, model.approvalComment).then(function (resp) {
              notify(resp, fromDash, item.nodeId);
            }, function (err) {
              notify(err, fromDash, item.nodeId);
            });
            workflowOverlay.close();
          },
          close: function close() {
            workflowOverlay.show = false;
            workflowOverlay = null;
          }
        };
        return workflowOverlay;
      },
      initiate: function initiate(name, id, publish) {
        var workflowOverlay = {
          view: overlayPath + "/workflow.submit.dialog.html",
          show: true,
          title: "Send for " + (publish ? 'publish' : 'unpublish') + " approval",
          subtitle: "Document: " + name,
          isPublish: publish,
          nodeId: id,
          submit: function submit(model) {
            _buttonState('busy', id);

            workflowResource.initiateWorkflow(id, model.comment, publish).then(function (resp) {
              notify(resp, false, id);
            }, function (err) {
              notify(err, false, id);
            });
            workflowOverlay.close();
          },
          close: function close() {
            workflowOverlay.show = false;
            workflowOverlay = null;
          }
        };
        return workflowOverlay;
      },
      cancel: function cancel(item, fromDash) {
        var workflowOverlay = {
          view: overlayPath + "/workflow.cancel.dialog.html",
          show: true,
          title: 'Cancel workflow process',
          subtitle: "Document: " + item.nodeName,
          comment: '',
          isFinalApproval: item.activeTask === 'Pending Final Approval',
          submit: function submit(model) {
            _buttonState('busy', item.nodeId);

            workflowResource.cancelWorkflowTask(item.instanceGuid, model.comment).then(function (resp) {
              notify(resp, fromDash, item.nodeId);
            });
            workflowOverlay.close();
          },
          close: function close() {
            workflowOverlay.show = false;
            workflowOverlay = null;
          }
        };
        return workflowOverlay;
      },
      detail: function detail(item) {
        var workflowOverlay = {
          view: overlayPath + "/workflow.action.dialog.html",
          show: true,
          title: 'Workflow detail',
          subtitle: "Document: " + item.nodeName,
          item: item,
          //comment: item.instanceComment,
          //guid: item.instanceGuid,
          //requestedBy: item.requestedBy,
          //requestedOn: item.requestedOn,
          detail: true,
          close: function close() {
            workflowOverlay.show = false;
            workflowOverlay = null;
          }
        };
        return workflowOverlay;
      },
      buttonState: function buttonState(state, id) {
        _buttonState(state, id);
      }
    };
    return service;
  }

  angular.module('plumber.services').factory('plmbrActionsService', ['$rootScope', 'plmbrWorkflowResource', 'notificationsService', workflowActionsService]);
})();

(function () {
  'use strict';

  function workflowGroupsResource($http, $q, umbRequestHelper) {
    var urlBase = Umbraco.Sys.ServerVariables.workflow.apiBasePath + '/groups';

    var request = function request(method, url, data) {
      return umbRequestHelper.resourcePromise(method === 'DELETE' ? $http.delete(url) : method === 'POST' ? $http.post(url, data) : method === 'PUT' ? $http.put(url, data) : $http.get(url), 'Something broke');
    };

    var service = {
      /**
       * @returns {array} user groups
       * @description Get single group by id, or all groups if no id parameter provided
       */
      get: function get(id) {
        return request('GET', urlBase + (id ? "/get/" + id : '/get'));
      },

      /**
       * @description Add a new group, where the param is the new group name
       * @param {string} name - the name of the new group
       * @returns the new user group
       */
      add: function add(name) {
        return request('POST', urlBase + '/add', {
          data: name
        });
      },

      /**
       * @param {object} group
       * @returns {string}
       * @description save updates to an existing group object
       */
      save: function save(group) {
        return request('PUT', urlBase + '/save', group);
      },

      /**
       * @param {int} id
       * @returns {string}
       * @description delete group by id
       */
      'delete': function _delete(id) {
        return request('DELETE', urlBase + '/delete/' + id);
      }
    };
    return service;
  }

  angular.module('plumber.services').factory('plmbrGroupsResource', ['$http', '$q', 'umbRequestHelper', workflowGroupsResource]);
})();

(function () {
  function plumberHub($rootScope, $q, assetsService) {
    var scripts = ['../App_Plugins/workflow/backoffice/lib/signalr/jquery.signalr-2.2.1.min.js', '/umbraco/backoffice/signalr/hubs'];

    function initHub(callback) {
      if ($.connection == undefined) {
        var promises = [];
        scripts.forEach(function (script) {
          promises.push(assetsService.loadJs(script));
        });
        $q.all(promises).then(function () {
          hubSetup(callback);
        });
      } else {
        hubSetup(callback);
      }
    }

    function hubSetup(callback) {
      var proxy = $.connection.plumberHub;
      var hub = {
        start: function start() {
          $.connection.hub.start();
        },
        on: function on(eventName, callback) {
          proxy.on(eventName, function (result) {
            $rootScope.$apply(function () {
              if (callback) {
                callback(result);
              }
            });
          });
        },
        invoke: function invoke(methodName, callback) {
          proxy.invoke(methodName).done(function (result) {
            $rootScope.$apply(function () {
              if (callback) {
                callback(result);
              }
            });
          });
        }
      };
      return callback(hub);
    }

    return {
      initHub: initHub
    };
  }

  angular.module('plumber.services').factory('plumberHub', ['$rootScope', '$q', 'assetsService', plumberHub]);
})();

(function () {
  'use strict'; // create service

  function workflowResource($http, $q, umbRequestHelper) {
    var activityFilter = void 0;
    var urlBase = Umbraco.Sys.ServerVariables.workflow.apiBasePath; // are there common elements between two arrays?

    var common = function common(arr1, arr2) {
      return arr1.some(function (el) {
        return arr2.indexOf(el) > -1;
      });
    };

    var request = function request(method, url, data) {
      return umbRequestHelper.resourcePromise(method === 'GET' ? $http.get(url) : $http.post(url, data), 'Something broke');
    };

    var urls = {
      settings: urlBase + '/settings',
      tasks: urlBase + '/tasks',
      instances: urlBase + '/instances',
      actions: urlBase + '/actions',
      logs: urlBase + '/logs',
      config: urlBase + '/config'
    };
    var service = {
      getContentTypes: function getContentTypes() {
        return request('GET', urls.settings + '/getcontenttypes');
      },

      /* tasks and approval endpoints */
      getApprovalsForUser: function getApprovalsForUser(userId, count, page) {
        return request('GET', urls.tasks + '/flows/' + userId + '/0/' + count + '/' + page);
      },
      getSubmissionsForUser: function getSubmissionsForUser(userId, count, page) {
        return request('GET', urls.tasks + '/flows/' + userId + '/1/' + count + '/' + page);
      },
      getPendingTasks: function getPendingTasks(count, page) {
        return request('GET', urls.tasks + '/pending/' + count + '/' + page);
      },
      getAllTasksForRange: function getAllTasksForRange(days) {
        return request('GET', urls.tasks + '/range/' + days);
      },
      getFilteredTasksForRange: function getFilteredTasksForRange(days, filter, count, page) {
        return request('GET', urls.tasks + '/filteredRange/' + days + (filter ? "/" + filter : '') + (count ? "/" + count : '') + (page ? "/" + page : ''));
      },
      getAllInstances: function getAllInstances(count, page, filter) {
        return request('GET', urls.instances + '/' + count + '/' + page + '/' + (filter || ''));
      },
      getAllInstancesForRange: function getAllInstancesForRange(days) {
        return request('GET', urls.instances + 'range/' + days);
      },
      getAllInstancesForNode: function getAllInstancesForNode(nodeId, count, page) {
        return request('GET', urls.instances + '/' + nodeId + '/' + count + '/' + page);
      },
      getFilteredInstancesForRange: function getFilteredInstancesForRange(days, filter, count, page) {
        return request('GET', urls.instances + '/filteredRange/' + days + (filter ? "/" + filter : '') + (count ? "/" + count : '') + (page ? "/" + page : ''));
      },
      getAllTasksForGroup: function getAllTasksForGroup(groupId, count, page) {
        return request('GET', urls.tasks + '/group/' + groupId + '/' + count + '/' + page);
      },
      getAllTasksByGuid: function getAllTasksByGuid(guid) {
        return request('GET', urls.tasks + '/tasksbyguid/' + guid);
      },
      getNodeTasks: function getNodeTasks(id, count, page) {
        return request('GET', urls.tasks + '/node/' + id + '/' + count + '/' + page);
      },
      getNodePendingTasks: function getNodePendingTasks(id) {
        return request('GET', urls.tasks + '/node/pending/' + id);
      },
      getTasks: function getTasks(id) {
        return request('GET', urls.tasks + '/get/' + id);
      },
      getStatus: function getStatus(ids) {
        return request('GET', urls.instances + '/status/' + ids);
      },
      workflowConfigured: function workflowConfigured() {
        return request('GET', urls.config + '/workflowconfigured');
      },

      /* workflow actions */
      initiateWorkflow: function initiateWorkflow(nodeId, comment, publish) {
        return request('POST', urls.actions + '/initiate', {
          nodeId: nodeId,
          comment: comment,
          publish: publish
        });
      },
      approveWorkflowTask: function approveWorkflowTask(instanceGuid, comment) {
        return request('POST', urls.actions + '/approve', {
          instanceGuid: instanceGuid,
          comment: comment
        });
      },
      rejectWorkflowTask: function rejectWorkflowTask(instanceGuid, comment) {
        return request('POST', urls.actions + '/reject', {
          instanceGuid: instanceGuid,
          comment: comment
        });
      },
      resubmitWorkflowTask: function resubmitWorkflowTask(instanceGuid, comment) {
        return request('POST', urls.actions + '/resubmit', {
          instanceGuid: instanceGuid,
          comment: comment
        });
      },
      cancelWorkflowTask: function cancelWorkflowTask(instanceGuid, comment) {
        return request('POST', urls.actions + '/cancel', {
          instanceGuid: instanceGuid,
          comment: comment
        });
      },

      /* get/set workflow settings*/
      getSettings: function getSettings() {
        return request('GET', urls.settings + '/get');
      },
      saveSettings: function saveSettings(settings) {
        return request('POST', urls.settings + '/save', settings);
      },
      getVersion: function getVersion() {
        return request('GET', urls.settings + '/version');
      },
      getDocs: function getDocs() {
        return request('GET', urls.settings + '/docs');
      },
      getLog: function getLog(date) {
        return request('GET', urls.logs + '/get/' + (date || ''));
      },
      getLogDates: function getLogDates() {
        return request('GET', urls.logs + '/datelist');
      },
      doImport: function doImport(model) {
        return request('POST', urlBase + '/import', model);
      },
      doExport: function doExport() {
        return request('GET', urlBase + '/export');
      },

      /* SAVE PERMISSIONS */
      saveConfig: function saveConfig(p) {
        return request('POST', urls.config + '/saveconfig', p);
      },
      saveDocTypeConfig: function saveDocTypeConfig(p) {
        return request('POST', urls.config + '/savedoctypeconfig', p);
      },
      checkExclusion: function checkExclusion(excludedNodes, path) {
        if (!excludedNodes) {
          return false;
        }

        var excluded = excludedNodes.split(','); // if any elements are shared, exclude the node from the workflow mechanism
        // by checking the path not just the id, this becomes recursive, and the excludeNodes cascades down the tree

        return common(path.split(','), excluded);
      },
      checkNodePermissions: function checkNodePermissions(groups, id, contentTypeAlias) {
        var resp = {
          approvalPath: [],
          contentTypeApprovalPath: []
        };
        groups.forEach(function (v) {
          v.permissions.forEach(function (p) {
            if (p.nodeId === id) {
              resp.approvalPath[p.permission] = v;
            }

            if (p.contentTypeAlias === contentTypeAlias) {
              resp.contentTypeApprovalPath[p.permission] = v;
            }
          });
        });
        return resp;
      },
      checkAncestorPermissions: function checkAncestorPermissions(path, groups) {
        // first is -1, last is the current node
        path = path.split(',');
        path.shift();
        path.pop();
        var resp = [];
        path.forEach(function (id) {
          groups.forEach(function (group) {
            group.permissions.forEach(function (p) {
              if (p.nodeId === parseInt(id, 10)) {
                resp[p.permission] = {
                  name: group.name,
                  groupId: p.groupId,
                  nodeName: p.nodeName,
                  permission: p.permission
                };
              }
            });
          });
        });
        return resp;
      },
      // pass the activity filter between the admin and history views
      setActivityFilter: function setActivityFilter(filter) {
        activityFilter = filter;
      },
      getActivityFilter: function getActivityFilter() {
        return activityFilter;
      }
    };
    return service;
  } // register service


  angular.module('plumber.services').factory('plmbrWorkflowResource', ['$http', '$q', 'umbRequestHelper', workflowResource]);
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

},{}]},{},[1]);
