
# AuthService API

Responsible for users identity, generates jwt and refresh tokens that are
user across whole system

## Features

- Cache
- RabbitMQ
- JWT Auth 
- Refresh Tokens
- Email/Phone Confirmation


# Built With
* ![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
* ![Redis](https://img.shields.io/badge/redis-%23DD0031.svg?style=for-the-badge&logo=redis&logoColor=white)
* ![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)
* ![Azure](https://img.shields.io/badge/azure-%230072C6.svg?style=for-the-badge&logo=azure-devops&logoColor=white)  
* ![JWT](https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=JSON%20web%20tokens)  
* ![RabbitMQ](https://img.shields.io/badge/rabbitmq-%23FF6600.svg?&style=for-the-badge&logo=rabbitmq&logoColor=white)  
* ![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Sever-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)

## Auth


| Header          | Usage                | Required                    |
|:----------------|:---------------------|:----------------------------|
| `x-api-key`     | `Key to the api`     | `Required by every enpoint` |
| `Authorization` | `Token to auth user` | `Required by every enpoint` |
                    


## Usage example

```http
  POST /api/Auth/Register
```
#####
```curl
curl -X 'POST' \
  'https://localhost:7277/api/Auth/Register' \
  -H 'accept: */*' \
  -H 'X-Api-Key: youapikey' \
  -H 'Content-Type: application/json' \
  -d '{
  "firstname": "string",
  "surname": "string",
  "password": "string",
  "confirmPassword": "string",
  "email": "string@gmail.com",
  "phoneNumber": "111222333",
  "deviceId": "string"
}'
```

### Response

```json
{
  "token": "signedJwtTokenHere",
  "refreshToken": "2d29aedf-23d5-483c-ae75-58b235b52c5d",
  "statusCode": 200,
  "issuedDate": "2023-07-12T21:54:39.080989+02:00",
  "expiresAt": "2023-07-12T23:54:39.0714333+02:00",
  "role": "User",
  "roleId": "CAE53EE9-6E7C-44B4-B2B3-D062B4A346F8",
  "userId": "6cb70bb2-156b-42e5-9431-ffa960fe4d20"
}
```

## Get Users
```http
  GET /api/Users
```
#####
```curl
curl -X 'GET' \
  'https://localhost:5001/api/User' \
  -H 'accept: */*' \
  -H 'X-Api-Key: youapikey'
```

### Response 

```json

{
  "items": [
    {
      "id": "2b43e38a-1bbe-4991-9cfb-80d110772097",
      "email": "test@gmail.com",
      "phoneNumber": "572370440",
      "deviceId": "string",
      "firstname": "fsdfs",
      "surname": "fsdfs"
    },
    {
      "id": "37af6524-e8df-4e5a-8399-cf10c9510f4e",
      "email": "solowiej@gmail.com",
      "phoneNumber": "572370440",
      "deviceId": null,
      "firstname": "Maciej",
      "surname": "Solowiej"
    }
  ],
  "totalPages": 1,
  "itemsFrom": 1,
  "itemsTo": 100,
  "totalItemsCount": 2
}

```
## Get User
```http
  GET /api/Emails/{id}
```
#####
```curl
curl -X 'GET' \
  'https://localhost:5001/api/User/c4faf909-df71-40b9-8885-2f4c26b63bc7' \
  -H 'accept: */*' \
  -H 'X-Api-Key: yourapikey'
```

### Response

```json

{
  "id": "c4faf909-df71-40b9-8885-2f4c26b63bc7",
  "email": "solowiejmaciej@gmail.com",
  "phoneNumber": "111222333",
  "deviceId": null,
  "firstname": "Maciej",
  "surname": "Sołowiej"
}

```

## Delete User
```http
  DELETE /api/User/{id}
```
#####
```curl
curl -X 'DELETE' \
  'https://localhost:7277/api/User/c4faf909-df71-40b9-8885-2f4c26b63bc7' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer signedJwt' \
  -H 'X-Api-Key: youapikey'
```

## Contact

- solowiejmaciej@gmail.com