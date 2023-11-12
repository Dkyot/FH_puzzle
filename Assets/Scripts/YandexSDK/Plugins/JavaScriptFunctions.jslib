mergeInto(LibraryManager.library, {
  getPlayerName: function () {
    try {
      var returnStr = player.getName();
      var bufferSize = lengthBytesUTF8(returnStr) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(returnStr, buffer, bufferSize);
      return buffer;
    } catch (err) {
      console.error(err);
      return null;
    }
  },

  getPlayerPhotoURL: function () {
    try {
      var returnStr = player.getPhoto("medium");
      var bufferSize = lengthBytesUTF8(returnStr) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(returnStr, buffer, bufferSize);
      return buffer;
    } catch (err) {
      console.error(err);
      return null;
    }
  },

  requestReviewGame: function () {
    ysdk.feedback.canReview().then(({ value, reason }) => {
      if (value) {
        ysdk.feedback.requestReview().then(({ feedbackSent }) => {
          console.log(feedbackSent);
        });
      } else {
        console.log(reason);
      }
    });
  },

  getReviewStatus: function () {
    try {
      ysdk.feedback.canReview().then(({ value, reason }) => {
        if (value) {
          return 0;
        } else {
          switch (reason) {
            case "NO_AUTH":
              return 1;
            case "GAME_RATED":
              return 2;
            case "REVIEW_ALREADY_REQUESTED":
              return 3;
            case "REVIEW_WAS_REQUESTED":
              return 4;
            case "UNKNOWN":
              return 1;
            default:
              return -1;
          }
        }
      });
    } catch (err) {
      console.error(err);
      return -1;
    }
  },

  savePlayerData: function (data) {
    try {
      var dateString = UTF8ToString(data);
      var myobj = JSON.parse(dateString);
      player.setData(myobj, true).then(() => {
        console.log("data is set");
      });
    } catch (err) {
      console.error(err);
    }
  },

  loadPlayerData: function (objectName, methodName) {
    var obj = UTF8ToString(objectName);
    var method = UTF8ToString(methodName);
    try {
      initPlayer().then((_player) => {
        player.getData().then((_date) => {
          var myJSON = JSON.stringify(_date);
          console.log(myJSON);
          unityInstance.SendMessage(obj, method, myJSON);
        });
      });
    } catch (error) {
      if (unityInstance === undefined) return;
      console.error(error);
      unityInstance.SendMessage(obj, method, '');
    }
  },

  setToLeaderboard: function (lbName, value) {
    try {
      var lbNameString = UTF8ToString(lbName);
      initLb().then((_lb) => {
        _lb.setLeaderboardScore(lbNameString, value);
      });
    } catch (err) {
      console.error(err);
    }
  },

  getLang: function () {
    try {
      var lang = ysdk.environment.i18n.lang;
      var bufferSize = lengthBytesUTF8(lang) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(lang, buffer, bufferSize);
      return buffer;
    } catch (err) {
      console.error(err);
      return null;
    }
  },

  showSplashPageAdv: function (objectName, methodName) {
    var obj = UTF8ToString(objectName);
    var method = UTF8ToString(methodName);
    console.log(obj);
    console.log(method);
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onOpen: function () {
          unityInstance.SendMessage(obj, method, 0);
        },
        onClose: function (wasShown) {
          unityInstance.SendMessage(obj, method, 1);
        },
        onError: function (error) {
          unityInstance.SendMessage(obj, method, -1);
        },
      },
    });
  },

  showRewardedAdv: function (objectName, methodName) {
    var obj = UTF8ToString(objectName);
    var method = UTF8ToString(methodName);
    console.log(obj);
    console.log(method);
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
          console.log("Video ad open.");
          unityInstance.SendMessage(obj, method, 0);
        },
        onRewarded: () => {
          console.log("Rewarded!");
          unityInstance.SendMessage(obj, method, 1);
        },
        onClose: () => {
          console.log("Video ad close.");
          unityInstance.SendMessage(obj, method, 2);
        },
        onError: (e) => {
          console.log("Error while open video ad:", e);
          unityInstance.SendMessage(obj, method, -1);
        },
      },
    });
  },

  apiReady: function () {
    try {
      ysdk.features.LoadingAPI.ready();
    } catch (err) {
      console.error(err);
    }
  },

  deviceType: function () {
    try {
      var returnStr = ysdk.deviceInfo.type;
      var bufferSize = lengthBytesUTF8(returnStr) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(returnStr, buffer, bufferSize);
      return buffer;
    } catch (err) {
      console.error(err);
      return null;
    }
  },

  callYandexMetric: function (goalName) {
    try {
      var name = UTF8ToString(goalName);
      ym(ymId, 'reachGoal', name);
    } catch (err) {
    }
  },
});
