const express = require("express");
const swaggerUi = require("swagger-ui-express");
const YAML = require("yamljs");
const app = express();

const swaggerDocument = YAML.load("./openapi.yaml");

app.use("/api-docs", swaggerUi.serve, swaggerUi.setup(swaggerDocument));

app.listen(3000, () => {
  console.log("API docs available at http://localhost:3000/api-docs");
});
