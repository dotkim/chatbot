const Soundex = require('soundex');

function compare(keyword, keywords) {
  let keySound = Soundex(keyword);
  let keywordObj = {};
  Object.keys(keywords).forEach((key) => {
    keywordObj[key] = Soundex(key);
  });
  Object.keys(keywordObj).forEach((key) => {
    if (keySound == keywordObj[key]) {
      console.log('found equal');
      console.log(key, keySound, keywordObj[key]);
      keyword = key;
    }
  });
  return keyword;
}

module.exports = compare;