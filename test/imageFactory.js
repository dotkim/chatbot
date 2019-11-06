/*eslint-disable*/
'use strict'
const ImageFactory = require('../src/lib/imageFactory');
const factory = new ImageFactory();
const assert = require('assert');
describe('ImageFactory', () => {

  //get function tests
  // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
  describe('#get()', () => {

    it('Should resolve to an Object', async () => {
      let res = await factory.get();
      assert.deepStrictEqual(typeof res, 'object');
    });

    it('should return a Promise', () => {
      assert.doesNotReject(factory.get());
    });

  });
  // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

});