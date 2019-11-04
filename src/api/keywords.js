'use strict'
const Request = require('./request');
const route = 'keywords'

class Keywords {
  get(keyword) {
    let req = new Request(route, keyword);
    return req.get()
  }

  add(keyword, message) {
    let req = new Request(route, keyword, {message});
    return req.post();
  }
}

module.exports = Keywords;