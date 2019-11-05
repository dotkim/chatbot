/*eslint-disable no-console*/
'use strict'

const Image = require('../src/lib/imageFactory');
const image = new Image();

const test = async function test() {
  try {
    let img = await image.get();
    console.log(img);
  } catch (err) {
    console.error(err);
  }
}

test();