'use strict'
const prefixes = {
  '!': 'keyword',
  '+': 'add',
  '.': 'help'
};

const Check = function Check(prefix) {
  if (!prefixes[prefix]) throw new Error('This prefix does not exist');
  return prefixes[prefix];
}

module.exports = Check;