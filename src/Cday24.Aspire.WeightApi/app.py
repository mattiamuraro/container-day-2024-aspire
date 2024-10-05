import os
from random import randint
from flask import Flask, request, jsonify
import logging

app = Flask(__name__)
logging.basicConfig()
logging.getLogger().setLevel(logging.NOTSET)

@app.route('/', methods=['GET'])
def hello_world():
    logging.info("request received on home!")
    return 'Hello, this is a demo python app for ContainerDay 2024!'

@app.route("/weight", methods=['GET'])
def weight_definition():
    result = randint(1, 1000)
    logging.info("Ticket receives weight of %s", result)

    return jsonify({"value": result})

if __name__ == '__main__':
    port = int(os.environ.get('PORT', 8111))
    app.run(host='0.0.0.0', port=port)