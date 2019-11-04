'use strict'
require('dotenv').config();

/**
 * returns a config object
 *
 * reads the .env file for the config
 * it sets the default value if nothing is found for that key
 * @class
 * @exports Config
 */
class Config {
  constructor() {
    this.config = {};
    //Discord bot token
    this.config.token = process.env.TOKEN || undefined;
    //if a domain name should be excluded from the image sniffer
    this.config.ignoredDomain = process.env.IGNOREDDOMAIN || undefined;
    //API info
    /*
     *If an URL is provided, the bot will send images to that API
     * The API description is not done yet, check https://github.com/dotkim/images-api
     * The auth is set on the API, currently only Basic auth
     */
    this.config.apiUrl = process.env.APIURL || undefined;
    this.config.apiPort = process.env.APIPORT || 80;
    this.config.apiAuth = process.env.APIAUTH || 'admin:secret';
  }
}

module.exports = new Config().config;