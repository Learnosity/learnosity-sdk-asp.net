﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Learnosity
{
    /**
     *--------------------------------------------------------------------------
     * Learnosity SDK - Init
     *--------------------------------------------------------------------------
     *
     * Used to generate the necessary security and request data (in the
     * correct format) to integrate with any of the Learnosity API services.
     *
     */

    public class Init
    {
        /**
         * Which Learnosity service to generate a request packet for.
         */
        public enum Service 
        {
            Assess,
            Author,
            Data,
            Items,
            Questions,
            Reports
        };

        /**
         * The consumer secret as provided by Learnosity. This is your private key
         * known only by the client (you) and Learnosity, which must not be exposed
         * either by sending it to the browser or across the network.
         * It should never be distributed publicly.
         */
        private string _secret;

        /**
         * An associative array of security details. This typically contains:
         *  - consumer_key
         *  - domain (optional depending on which service is being intialised)
         *  - timestamp (optional)
         *  - user_id (optional depending on which service is being intialised)
         *
         * It's important that the consumer secret is NOT a part of this array.
         */
        private Dictionary<string, string> _securityPacket = new Dictionary<string,string>();

        /**
         * An optional object of request parameters used as part
         * of the service (API) initialisation.
         */
        private string _requestPacket;

        /**
         * If `requestPacket` is used, `requestString` will be the string
         * (JSON) representation of that. It's used to create the signature
         * and returned as part of the service initialisation data.
         */
        private string _requestString;

        /**
         * An optional value used to define what type of request is being
         * made. This is only required for certain requests made to the
         * Data API (http://docs.learnosity.com/dataapi/)
         */
        public enum Action 
        {
            GET,
            POST
        };

        /**
         * Most services add the request packet (if passed) to the signature
         * for security reasons. This flag can override that behaviour for
         * services that don't require this.
         */
        private bool _signRequestData = true;

        /**
         * Keynames that are valid in the securityPacket, they are also in
         * the correct order for signature generation.
         * @var array
         */
        private string[] _validSecurityKeys = new string[] {"consumer_key", "domain", "timestamp", "user_id"};

        /**
         * Service names that are valid for `service`
         * @var array
         */
        private string[] _validServices = new string[] {"assess", "author", "data", "items", "questions", "reports"};

        /**
         * The algorithm used in the hashing function to create the signature
         */
        private string _algorithm = "sha256";

        /**
         * Instantiate this class with all security and request data. It
         * will be used to create a signature.
         *
         * @param string   service
         * @param mixed    securityPacket
         * @param string   secret
         * @param mixed    requestPacket
         * @param string   action
         */
        public string Init(enum Service, Dictionary<string, string> securityPacket, string secret, string requestPacket, enum Action)
        {
            //try and catch used in PHP, strong typing should remove need along with the validation removal?
            // First validate the arguments passed
            //this->validate(service, securityPacket, secret, requestPacket, action);

                // Set instance variables based off the arguments passed
                this.Service         = Service;
                this._securityPacket = securityPacket;
                this._secret         = secret;
                this._requestPacket  = requestPacket;
                this._requestString  = this.generateRequestString();
                this.Action          = Action;

                // Set any service specific options
                this.setServiceOptions();

                // Generate the signature based on the arguments provided
                this.securityPacket['signature'] = this.generateSignature();
        }

        /**
         * Generate the data necessary to make a request to one of the
         * Learnosity products/services.
         *
         * @return string A JSON string
         */
        public function generate()
        {
            output = array();

            switch (this->service) {
                case 'assess':
                case 'author':
                case 'data':
                case 'items':
                case 'reports':
                    // Add the security packet (with signature) to the output
                    output['security'] = this->securityPacket;

                    // Stringify the request packet if necessary
                    if (!empty(this->requestPacket)) {
                        output['request'] = this->requestPacket;
                    }

                    // Add the action if necessary (Data API)
                    if (!empty(this->action)) {
                        output['action'] = this->action;
                    }

                    if (this->service === 'data') {
                        r = 'security=' . Json::encode(output['security']);
                        if (array_key_exists('request', output)) {
                            r .= '&request=' . Json::encode(output['request']);
                        }
                        if (array_key_exists('action', output)) {
                            r .= '&action=' . Json::encode(output['action']);
                        }
                        return r;
                    } elseif (this->service === 'assess') {
                        output = output['request'];
                    }
                    break;
                case 'questions':
                    // Add the security packet (with signature) to the root of output
                    output = this->securityPacket;

                    // Remove the `domain` key from the security packet
                    unset(output['domain']);

                    // Stringify the request packet if necessary
                    if (!empty(this->requestPacket)) {
                        output = array_merge_recursive(output, this->requestPacket);
                    }
                    break;
                default:
                    // no default
                    break;
            }

            return Json::encode(output);
        }

        /**
         * Generate a JSON string from the requestPacket (array) or null
         * if no requestPacket is required for this request
         *
         * @return mixed
         */
        private function generateRequestString()
        {
            if (empty(this->requestPacket)) {
                return null;
            }
            requestString = Json::encode(this->requestPacket);
            if (false === requestString) {
                throw new Exception('Invalid data, please check your request packet - ' . Json::checkError());
            }
            return requestString;
        }

        /**
         * Generate a signature hash for the request, this includes:
         *  - the security credentials
         *  - the `request` packet (a JSON string) if passed
         *  - the `action` value if passed
         *
         * @return string A signature hash for the request authentication
         */
        public function generateSignature()
        {
            signatureArray = array();

            // Create a pre-hash string based on the security credentials
            // The order is important
            foreach (this->validSecurityKeys as key) {
                if (array_key_exists(key, this->securityPacket)) {
                    array_push(signatureArray, this->securityPacket[key]);
                }
            }

            // Add the secret
            array_push(signatureArray, this->secret);

            // Add the requestPacket if necessary
            if (this->signRequestData && !empty(this->requestString)) {
                array_push(signatureArray, this->requestString);
            }

            // Add the action if necessary
            if (!empty(this->action)) {
                array_push(signatureArray, this->action);
            }

            return this->hashValue(signatureArray);
        }

        /**
         * Hash an array value
         *
         * @param  array  value An array to hash
         *
         * @return string        The hashed string
         */
        private function hashValue(value)
        {
            return hash(this->algorithm, implode('_', value));
        }

        /**
         * Set any options for services that aren't generic
         */
        private function setServiceOptions()
        {
            switch (this->service) {
                case 'assess':
                case 'author':
                case 'questions':
                    this->signRequestData = false;
                    // The Assess API holds data for the Questions API that includes
                    // security information and a signature. Retrieve the security
                    // information from this and generate a signature for the
                    // Questions API
                    if (
                        this->service === 'assess' &&
                        array_key_exists('questionsApiActivity', this->requestPacket)
                    ) {
                        questionsApi = this->requestPacket['questionsApiActivity'];
                        domain = 'assess.learnosity.com';
                        if (array_key_exists('domain', this->securityPacket)) {
                            domain = this->securityPacket['domain'];
                        } elseif (array_key_exists('domain', questionsApi)) {
                            domain = questionsApi['domain'];
                        }

                        this->requestPacket['questionsApiActivity'] = array(
                            'consumer_key' => this->securityPacket['consumer_key'],
                            'timestamp'    => this->securityPacket['timestamp'],
                            'user_id'      => this->securityPacket['user_id'],
                            'signature'    => this->hashValue(
                                array(
                                    'consumer_key' => this->securityPacket['consumer_key'],
                                    'domain'       => domain,
                                    'timestamp'    => this->securityPacket['timestamp'],
                                    'user_id'      => this->securityPacket['user_id'],
                                    'secret'       => this->secret
                                )
                            )
                        );
                        unset(questionsApi['consumer_key']);
                        unset(questionsApi['domain']);
                        unset(questionsApi['timestamp']);
                        unset(questionsApi['user_id']);
                        unset(questionsApi['signature']);
                        this->requestPacket['questionsApiActivity'] = array_merge(
                            this->requestPacket['questionsApiActivity'],
                            questionsApi
                        );
                    }
                    break;
                case 'items': {
                    // The Events API requires a user_id, so we make sure it's a part
                    // of the security packet as we share the signature in some cases
                    if (
                        !array_key_exists('user_id', this->securityPacket) &&
                        array_key_exists('user_id', this->requestPacket)
                    ) {
                        this->securityPacket['user_id'] = this->requestPacket['user_id'];
                    }
                }
                default:
                    // do nothing
                    break;
            }
        }

        /**
         * Validate the arguments passed to the constructor
         *
         * @param  string   service
         * @param  array    securityPacket
         * @param  string   secret
         * @param  array    requestPacket
         * @param  string   action
         */
        public function validate(service, &securityPacket, secret, &requestPacket, action)
        {
            if (empty(service)) {
                throw new Exception('The `service` argument wasn\'t found or was empty');
            } elseif (!in_array(strtolower(service), this->validServices)) {
                throw new Exception("The service provided (service) is not valid");
            }

            // In case the user gave us a JSON securityPacket, convert to an array
            if (!is_array(securityPacket) && is_string(securityPacket)) {
                securityPacket = json_decode(securityPacket, true);
            }

            if (empty(securityPacket) || !is_array(securityPacket)) {
                throw new Exception('The security packet must be an array');
            } else {
                foreach (array_keys(securityPacket) as key) {
                    if (!in_array(key, this->validSecurityKeys)) {
                        throw new Exception('Invalid key found in the security packet: ' . key);
                    }
                }
                if (!array_key_exists('timestamp', securityPacket)) {
                    securityPacket['timestamp'] = gmdate('Ymd-Hi');
                }
            }

            if (empty(secret) || !is_string(secret)) {
                throw new Exception('The `secret` argument must be a valid string');
            }

            // In case the user gave us a JSON requestPacket, convert to an array
            if (!is_array(requestPacket) && is_string(requestPacket)) {
                requestPacket = json_decode(requestPacket, true);
                if (empty(requestPacket)) {
                    throw new Exception('Invalid data, please check your request packet - ' . Json::checkError());
                }
            }

            if (!empty(requestPacket) && !is_array(requestPacket)) {
                throw new Exception('The request packet must be an array');
            }

            if (!empty(action) && !is_string(action)) {
                throw new Exception('The action parameter must be a string');
            }
        }
    }
}