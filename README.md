# Booking API

This application provides a REST API to book a Hotel.

## Technologies

| Name | Description |
|---|---|
| `.NET 5` | <https://github.com/dotnet/efcore> |
| `Sql Server` | <https://docs.microsoft.com/pt-br/sql/sql-server/?view=sql-server-ver15> |

## Libraries

| Name | Description |
|---|---|
| `Auto Mapper` | <https://github.com/AutoMapper/AutoMapper> |
| `Dapper` | <https://github.com/DapperLib/Dapper> |
| `EF Core 5` | <https://github.com/dotnet/docs>. |
| `Swagger` | <https://github.com/swagger-api> |
| `xUnit` | <https://github.com/xunit/xunit> |
| `Moq` | <https://github.com/moq/moq> |
| `Moq.Dapper` | <https://github.com/UnoSD/Moq.Dapper> |

## Docs
You can see API structure (only development enviroment) on: `https://localhost:5001/swagger/index.html`

## Requirements

[dotnet tools](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-tool-install)

## Install

You can create the database using `database-update.bat`.
Open `cmd` and run:

     cd `<app_root>`\tools

Then run:

     database-update.bat

## Resources

* [Bookings](#bookings)
* [Calendars](#calendars)

### Verbs

| Verb | Description |
|---|---|
| `GET` | It returns information of one or more records. |
| `POST` | It's used to create a new record. |
| `PUT` | It updates data of a record or change its situation. |
| `DELETE` | It removes a record from the system. |

### Status Codes

| Code | Descriiption |
|---|---|
| `200` | Request successfully executed (success).|
| `204` | When the record isn't found or the server has fulfilled the request but does not need to return an entity-body.|
| `400` | Validation errors or the informed fields don't exist in the system.|
| `500` | It means that the server encountered an unexpected condition that prevented it from fulfilling the request.|

## Bookings

### Get - Booking

***Request***

`GET /bookings/{bookingId}`

    curl -X GET "https://localhost:5001/bookings/D493A222-C84A-4379-F7D9-08D9BCCBA149" -H  "accept: text/plain"

***Response***

**200**

Header

    content-type: application/json; charset=utf-8 

Body

    {
      "id": "50e1a856-000d-4ce3-3f17-08d9bce48e50",
      "checkIn": "2021-12-27",
      "checkOut": "2021-12-28"
    }

**204**

Header

    N/A 

Body

    N/A

**500**

Header

    content-type: application/json; charset=utf-8 

Body

    {
      "errors": [
          "The operation was aborted."
      ]
    }

### Put - Booking

***Request***

`PUT /bookings/{bookingId}`

    curl -X PUT "https://localhost:5001/bookings/B5EA3DB8-2251-4F26-82FA-08D9BD063D91" -H  "accept: */*" -H  "Content-Type: application/json" -d "{\"checkIn\":\"2021-12-29\",\"checkOut\":\"2021-12-29\"}"

Body

    {
      "checkIn": "2021-12-27",
      "checkOut": "2021-12-29"
    }

***Response***

**204**

Header

    N/A 

Body

    N/A

**400**

Header

    content-type: application/json; charset=utf-8 

Body

    {
      "errors": [
        "The resevation couldn't be updated."
      ]
    }

**500**

Header

    content-type: application/json; charset=utf-8 

Body

    {
      "errors": [
          "The operation was aborted."
      ]
    }

### Delete - Booking

***Request***

`DELETE /bookings/{bookingId}`

    curl -X DELETE "https://localhost:5001/bookings/B5EA3DB8-2251-4F26-82FA-08D9BD063D91" -H  "accept: */*"
    
***Response***

**204**

Header

    N/A 

Body

    N/A

**400**

Header

    content-type: application/json; charset=utf-8 

Body

    {
      "errors": [
        "The resevation b5ea3db8-2251-4f26-82fa-08d9bd063d91 doesn't exist."
        ]
    }

**500**

Header

    content-type: application/json; charset=utf-8 

Body

    {
      "errors": [
          "The operation was aborted."
      ]
    }

### Post - Booking

***Request***

`Post /bookings`

    curl -X POST "https://localhost:5001/bookings" -H  "accept: */*" -H  "Content-Type: application/json" -d "{\"checkIn\":\"2021-12-29\",\"checkOut\":\"2021-12-29\"}" 

Body

    {
      "checkIn": "2021-12-27",
      "checkOut": "2021-12-29"
    }

***Response***

**200**

Header

    content-type: application/json; charset=utf-8 

Body

    {
      "bookingId": "379ae404-f992-46a7-888d-2a8c1eea8631"
    }

**400**

Header

    content-type: application/json; charset=utf-8 

Body

    {
      "errors": [
        "The CheckIn can't be earlier than 2021-12-13."
      ]
    }

**500**

Header

    content-type: application/json; charset=utf-8 

Body

    {
      "errors": [
          "The operation was aborted."
      ]
    }

## Calendars

### Get - Calendar

***Request***

`GET /calendar/available-dates`

    curl -X GET "https://localhost:5001/calendars/dates/available-dates" -H  "accept: text/plain"

***Response***

**200**

Header

    content-type: application/json; charset=utf-8 

Body

    [
      {
        "date": "2021-12-13",
        "isAvailable": false
      }
    ]

**204**

Header

    N/A 

Body

    N/A

**500**

Header

    content-type: application/json; charset=utf-8 

Body

    {
      "errors": [
          "The operation was aborted."
      ]
    }
