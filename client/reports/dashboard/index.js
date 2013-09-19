// Generated by CoffeeScript 1.6.3
(function() {
  var CacheItem, Chart, CurrentCacheItems, DefaultExpirationMilliseconds, GetCacheItem,
    __bind = function(fn, me){ return function(){ return fn.apply(me, arguments); }; };

  require.config({
    baseUrl: '',
    map: {
      '*': {
        'css': '/libs/require-css/css',
        'text': '/libs/require-text'
      }
    }
  });

  DefaultExpirationMilliseconds = 1 * 3600 * 1000;

  CurrentCacheItems = {};

  GetCacheItem = function(key) {
    var existing;
    existing = CurrentCacheItems[key];
    if (existing) {
      return existing;
    }
    CurrentCacheItems[key] = new CacheItem(key, DefaultExpirationMilliseconds);
    return CurrentCacheItems[key];
  };

  CacheItem = (function() {
    function CacheItem(key, expirationMilliseconds) {
      this.key = key;
      this.expirationMilliseconds = expirationMilliseconds;
    }

    CacheItem.prototype.data = null;

    CacheItem.prototype.lastUpdate = 0;

    CacheItem.prototype.setData = function(newData) {
      this.lastUpdate = new Date();
      return this.data = newData;
    };

    CacheItem.prototype.isValid = function() {
      return (new Date() - this.lastUpdate) < this.expirationMilliseconds;
    };

    CacheItem.prototype.invalidate = function() {
      return this.lastUpdate = 0;
    };

    return CacheItem;

  })();

  Chart = (function() {
    function Chart(api, selector, maker, map) {
      this.api = api;
      this.selector = selector;
      this.maker = maker;
      this.map = map;
      this.update = __bind(this.update, this);
      this.chart = maker(this);
    }

    Chart.prototype.update = function(from, to, api) {
      var cacheItem, self;
      if (api == null) {
        api = this.api;
      }
      api += "?from=" + from + "&to=" + to;
      self = this;
      cacheItem = GetCacheItem(api);
      if (!cacheItem.isValid()) {
        return d3.json(api, function(raw) {
          cacheItem.setData(raw);
          return d3.select(self.selector + " .chart").datum(self.map(raw)).call(self.chart);
        });
      } else {
        return d3.select(self.selector + " .chart").datum(self.map(cacheItem.data)).call(self.chart);
      }
    };

    return Chart;

  })();

  require(['../../chart-modules/bar/chart.js', '../../chart-modules/pie/chart.js', '../../chart-modules/common/d3-tooltip.js'], function(barChart, pieChart, tooltip) {
    var charts, customDateRanges, getDate, setFrom, shiftDate, shiftDateUI, shiftDatesMinus, shiftDatesPlus, updateChart, updateCharts, usersChart, ymdDate;
    charts = [];
    charts.push({
      api: "http://stats.macademy.com/Home/QuickStats",
      render: function(stats) {
        return ['Visits', 'Users', 'Purchases'].forEach(function(i) {
          $("#QuickStats [data-raw] [data-" + i + "]").text(d3.format(',f')(stats[i]));
          return $("#QuickStats [data-ratio] [data-" + i + "]").text(d3.format('.2p')(stats[i] / stats.Visits));
        });
      },
      update: function(from, to) {
        var api, cache, self;
        self = this;
        api = this.api + ("?from=" + from + "&to=" + to);
        cache = GetCacheItem(api);
        if (cache.isValid()) {
          return self.render(cache.data);
        } else {
          return d3.json(api, function(stats) {
            cache.setData(stats[0]);
            return self.render(stats[0]);
          });
        }
      }
    });
    charts.push(new Chart("http://stats.macademy.com/Home/TrialChapterVisits", "#UsersTrialChapterVisits", (function() {
      return pieChart().margin({
        right: 120
      }).width(300).height(300).colors(d3.scale.category10());
    }), function(raw) {
      var d;
      d = raw[0];
      return [
        {
          name: 'Visited Trial Chapter',
          value: d['Visits']
        }, {
          name: '',
          value: d['Users'] - d['Visits']
        }
      ];
    }));
    charts.push(new Chart("http://stats.macademy.com/Home/TrialChapterVisitsByBuyers", "#BuyersTrialChapterVisits", (function() {
      return pieChart().margin({
        right: 120
      }).width(300).height(300).colors(d3.scale.category10());
    }), function(raw) {
      var d;
      d = raw[0];
      return [
        {
          name: 'Visited Trial Chapter',
          value: d['Visits']
        }, {
          name: '',
          value: d['Users'] - d['Visits']
        }
      ];
    }));
    charts.push(new Chart("http://stats.macademy.com/Home/IapEventsPerUser", "#IapEventsPerUser", (function(self) {
      return barChart().width(700).height(350).devs(function() {
        return 0;
      }).tooltip(tooltip().text(function(d) {
        return d3.format('.2p')(d.value / self.total);
      })).margin({
        bottom: 40
      }).funnel(true);
    }), function(raw) {
      var data;
      data = _(raw[0]).map(function(value, key) {
        return {
          name: key,
          value: value
        };
      });
      this.total = data[0].value;
      return data;
    }));
    charts.push(new Chart("http://stats.macademy.com/Home/IapRequestsBeforePurchaseHistogram", "#IapRequestsBeforePurchaseHistogram", (function(self) {
      return barChart().tooltip(tooltip().text(function(d) {
        return d3.format('.2p')(d.value / self.total);
      })).devs(function() {
        return 0;
      }).margin({
        bottom: 45,
        left: 50
      }).width(465).height(300).drawExpectedValue(true).coalescing(10).xAxis({
        text: "Page Visits",
        dy: "-.35em"
      }).yAxis({
        text: "Users",
        dy: ".75em"
      });
    }), function(raw) {
      var data, extent, _i, _ref, _ref1, _results;
      extent = d3.extent(raw.map(function(d) {
        return d['Requests'];
      }));
      raw = (function() {
        _results = [];
        for (var _i = _ref = extent[0], _ref1 = extent[1]; _ref <= _ref1 ? _i <= _ref1 : _i >= _ref1; _ref <= _ref1 ? _i++ : _i--){ _results.push(_i); }
        return _results;
      }).apply(this).map(function(requests) {
        return (raw.filter(function(r) {
          return r['Requests'] === requests;
        }))[0] || {
          Requests: requests,
          Users: 0
        };
      });
      data = _(raw).map(function(d) {
        return {
          name: d['Requests'],
          value: d['Users']
        };
      });
      this.total = data.map(function(d) {
        return d.value;
      }).reduce(function(a, b) {
        return a + b;
      });
      return data;
    }));
    charts.push(new Chart("http://stats.macademy.com/Home/ViewOfPurchase", "#ViewOfPurchase", (function() {
      return pieChart().margin({
        right: 120
      }).width(465).height(300).colors(d3.scale.category10());
    }), function(raw) {
      return _(raw).map(function(d) {
        return {
          name: d['View'],
          value: d['Count']
        };
      });
    }));
    usersChart = new Chart("http://stats.macademy.com/Home/AppUsersSources", "#AppUsersSources", (function() {
      return pieChart().margin({
        right: 120
      }).width(465).height(300).colors(function(e) {
        return {
          'facebok': '#1f77b4',
          'none': 'gray',
          'form': 'green'
        }[e];
      });
    }), function(raw) {
      return _(raw).map(function(d) {
        var _ref;
        return {
          name: (_ref = d['Source']) != null ? _ref : "none",
          value: d['Users']
        };
      });
    });
    $("input[name=buyers]").on("change", function() {
      return updateChart(usersChart, ("true" === $(this).val() ? "http://stats.macademy.com/Home/AppUsersSourcesAtPurchase" : "http://stats.macademy.com/Home/AppUsersSources"));
    });
    charts.push(usersChart);
    charts.push(new Chart("http://stats.macademy.com/Home/VisitsBeforePurchaseHistogram", "#VisitsBeforePurchaseHistogram", (function(self) {
      return barChart().width(465).height(300).devs(function() {
        return 0;
      }).tooltip(tooltip().text(function(d) {
        return d3.format('.2p')(d.value / self.total);
      })).margin({
        bottom: 45,
        left: 50
      }).drawExpectedValue(true).coalescing(10).xAxis({
        text: "Page Visits",
        dy: "-.35em"
      }).yAxis({
        text: "Users",
        dy: ".75em"
      });
    }), function(raw) {
      var data, extent, _i, _ref, _ref1, _results;
      extent = d3.extent(raw.map(function(d) {
        return d['Visits'];
      }));
      raw = (function() {
        _results = [];
        for (var _i = _ref = extent[0], _ref1 = extent[1]; _ref <= _ref1 ? _i <= _ref1 : _i >= _ref1; _ref <= _ref1 ? _i++ : _i--){ _results.push(_i); }
        return _results;
      }).apply(this).map(function(visits) {
        return (raw.filter(function(r) {
          return r['Visits'] === visits;
        }))[0] || {
          Visits: visits,
          Users: 0
        };
      });
      data = _(raw).map(function(d) {
        return {
          name: d['Visits'],
          value: d['Users']
        };
      });
      this.total = data.map(function(d) {
        return d.value;
      }).reduce(function(a, b) {
        return a + b;
      });
      return data;
    }));
    charts.push(new Chart("http://stats.macademy.com/Home/UsageAfterPurchase", "#UsageAfterPurchase", (function(self) {
      return barChart().width(700).height(350).devs(function() {
        return 0;
      }).tooltip(tooltip().text(function(d) {
        return d3.format('.2p')(d.value / self.total);
      })).margin({
        bottom: 45,
        left: 50
      }).drawExpectedValue(true).coalescing(20).xAxis({
        text: "Page Visits",
        dy: "-.35em"
      }).yAxis({
        text: "Users",
        dy: ".75em"
      });
    }), function(raw) {
      var data, extent, _i, _ref, _ref1, _results;
      extent = d3.extent(raw.map(function(d) {
        return d['Visits'];
      }));
      raw = (function() {
        _results = [];
        for (var _i = _ref = extent[0], _ref1 = extent[1]; _ref <= _ref1 ? _i <= _ref1 : _i >= _ref1; _ref <= _ref1 ? _i++ : _i--){ _results.push(_i); }
        return _results;
      }).apply(this).map(function(visits) {
        return (raw.filter(function(r) {
          return r['Visits'] === visits;
        }))[0] || {
          Visits: visits,
          Users: 0
        };
      });
      data = _(raw).map(function(d) {
        return {
          name: d['Visits'],
          value: d['Users']
        };
      });
      this.total = data.map(function(d) {
        return d.value;
      }).reduce(function(a, b) {
        return a + b;
      });
      return data;
    }));
    ymdDate = d3.time.format("%Y-%m-%d");
    $("input[type=date]").attr("max", ymdDate(new Date(new Date().getTime() + 3600 * 1000 * 24 * 1)));
    $("#fromDate").val(ymdDate(new Date(new Date().getTime() - 3600 * 1000 * 24 * 1)));
    $("#toDate").val(ymdDate(new Date(new Date().getTime() + 3600 * 1000 * 24 * 1))).change();
    updateCharts = function() {
      return charts.forEach(function(chart) {
        return updateChart(chart);
      });
    };
    updateChart = function(chart, api) {
      if (api == null) {
        api = null;
      }
      return chart.update($("#fromDate").val(), $("#toDate").val(), api);
    };
    $("input[type=date]").on('change', function() {
      return updateCharts();
    });
    updateCharts();
    getDate = function(stringYMDDate) {
      var fDate, fDateArray;
      fDateArray = stringYMDDate.split("-");
      return fDate = new Date(fDateArray[0], fDateArray[1] - 1, fDateArray[2]);
    };
    shiftDate = function(stringYMDDate, milliShift) {
      var fDate;
      fDate = getDate(stringYMDDate);
      return new Date(fDate.getTime() + milliShift);
    };
    shiftDateUI = function(amount) {
      var from, to;
      from = shiftDate($("#fromDate").val(), amount);
      to = shiftDate($("#toDate").val(), amount);
      $("#fromDate").val(ymdDate(from));
      return $("#toDate").val(ymdDate(to)).change();
    };
    shiftDatesMinus = function() {
      return shiftDateUI(-3600 * 24 * 1000);
    };
    shiftDatesPlus = function() {
      return shiftDateUI(3600 * 24 * 1000);
    };
    setFrom = function(from) {
      $("#fromDate").val(ymdDate(from));
      return $("#toDate").val(ymdDate(shiftDate($("#fromDate").val(), 24 * 3600 * 1000))).change();
    };
    $("#todayDateMinus").click(shiftDatesMinus);
    $("#todayDatePlus").click(shiftDatesPlus);
    $("#todayDate").click(function() {
      return setFrom(new Date());
    });
    customDateRanges = {
      '0': '2013-09-16',
      '1': '2013-09-14',
      '2': '2013-09-15'
    };
    $(".windowsPhoneStyleBtns").each(function() {
      $(this).mouseover(function() {
        return $(this).addClass('hover');
      });
      return $(this).mouseout(function() {
        return $(this).removeClass('hover');
      });
    });
    return $("input[id^=app]").each(function(i, e) {
      return $(e).click(function() {
        $("input[id^=app]").removeClass('active');
        setFrom(getDate(customDateRanges[i]));
        return $(this).addClass('active');
      });
    });
  });

}).call(this);

/*
//@ sourceMappingURL=index.map
*/
