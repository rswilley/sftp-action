# SSH for GitHub Actions with .NET 6

<a href="https://github.com/features/actions">GitHub Action</a> for executing remote ssh commands built with <a href="https://github.com/docker">Docker</a>, <a href="https://www.nuget.org/packages/CommandLineParser/">CommandLineParser</a>, and <a href="https://github.com/sshnet/SSH.NET/">SSH.NET</a>.

This makes some assumptions on how your SSH is setup based on my settings. We are using a private key to authenticate.

## Input Variables

See <a href="https://github.com/rswilley/ssh-action/blob/master/action.yml">action.yml</a> for more detailed information.

* ```hostname``` ssh host
* ```username``` ssh user
* ```command``` command(s) to run

## Environment Variables

See <a href="https://github.com/rswilley/ssh-action/blob/master/example.yml">example.yml</a> for more detailed information

* ```PRIVATEKEY``` ssh private key

## Usage

Executing remote SSH commands.

### Single Command

```
name: remote ssh command
on: [push]
jobs:

  build:
    name: Build
    runs-on: ubuntu-latest
    steps:
    - name: executing remote ssh commands
      uses: rswilley/ssh-action@master
      with:
        hostname: ${{ secrets.HOSTNAME }}
        username: ${{ secrets.USERNAME }}
        command: whoami
```

### Multiple Commands

```
name: remote ssh command
on: [push]
jobs:

  build:
    name: Build
    runs-on: ubuntu-latest
    steps:
    - name: executing remote ssh commands
      uses: rswilley/ssh-action@master
      with:
        hostname: ${{ secrets.HOSTNAME }}
        username: ${{ secrets.USERNAME }}
        command: |
          whoami
          ls -la
```

## Setting up a SSH Key

You should be able to use any SSH Key type that SSH.NET supports, but the below method is tested as working.

```ssh-keygen -t ed25519 -C "your_email@example.com"```
