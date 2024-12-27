# Touch the Wacom Ring
_For OpenTabletDriver users with Wacom PTH-x60_

## Introduction
Wacom PTH-x60 (the 2017 Intuos Pro) pen tablets have this fun little toy called
"touch ring". However, OpenTabletDriver 0.6.5 currently does not make use of it
so it is kind of useless for OTD users.

This is OTD plugin that make use of touch rings so that you can use it to
control brush size for example, or scrolling the webpage.

## Filters
This OTD plugin introduces 1 filter:

- Touch Ring Keybind: Bind keyboard button to ring spinning action. Left button
if user is spinning to the left, right button if user is spinning to the right,
unit press mode basically presses based on spin distance, while report mode
presses on every report received from tablet (up to 20 presses a second).

## Installation
### Download and install
Not available at this moment.

### Build manually
You'll need .NET 8.0 SDK. Clone this repository, build like normal class lib,
then copy entire `net8.0/Debug` folder to your plugin folder and rename the
folder to something like "TouchTheWacomRing".

## Understanding the touch ring from reports
`Raw[4]` is the only byte that is being used by touch ring (from my observation
so far). The value is `0x7F` if user no longer touching the ring and anything
from `0x80` to `0xC7` if user is touching the ring. However, if user leave
their finger there without moving, the tablet will automatically reset back to
`0x7F` after unspecified amount of time, which means it will reports that user
is no longer touching the ring.

The resolution of touch ring is 72, or can be known as 72 divisions on the ring

## License
MIT License.