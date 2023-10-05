# DO NOT USE, THIS PROJECT IS VERY EARLY AND WIP

# Udon Soup

VRChat World Framework

## The Goal

This is an attempt to make a general purpose udon sharp framework to organise and manage your worlds. The framework is based on MVC (Model, Controller, View) frameworks to separate and structure your code.

## Definitions

### Router

The router is the entry point for all local and networked events. It determines which `Controller` should handle the event.

### Controller

The controller is for handling an indivisual event, this if for orchestrating how the event is handled but the actual handling will be done by other objects, like the `Model`

### Model

The model represent the data and the business logic for your world. This controls local and network synced data and broadcasting state changes to this data.

### View

The view represents the visual layer of your world, the buttons, text boxes, guns, cars, etc. These are used to fire off events for the `Router` and to handle state changes from the `Model`.
