/* eslint-disable */
const KeywordFactory = require('../src/lib/keywordFactory');
const factory = new KeywordFactory();

(async function() {
  let keyword = await factory.add('kim', 'pappa');
  console.log(`Received object: ${keyword.schema}\nKeyword: ${keyword.schema.keyword}\nValues: ${keyword.schema.values}`);
})();