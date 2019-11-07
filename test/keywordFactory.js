/*eslint-disable*/
'use strict'
const KeywordFactory = require('../src/lib/keywordFactory');
const factory = new KeywordFactory();
const assert = require('assert');
describe('KeywordFactory', () => {

  //get function tests
  // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
  describe('#get()', () => {

    it('Should resolve to an Object', (done) => {
      let res = factory.get('kim', done());
      assert.deepStrictEqual(typeof res, 'object');
    });

    it('should return a Promise', () => {
      assert.doesNotReject(factory.get('kim'));
    });

  });
  // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

});