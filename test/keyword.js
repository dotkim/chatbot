/* eslint-disable */
const KeywordFactory = require('../src/lib/keywordFactory');
const factory = new KeywordFactory();

(async function() {
  let keyword = await factory.add('kim', 'mamma');
  console.log(keyword.schema);
})();