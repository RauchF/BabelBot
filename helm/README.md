# babelbot

![Version: 0.1.0](https://img.shields.io/badge/Version-0.1.0-informational?style=flat-square) ![Type: application](https://img.shields.io/badge/Type-application-informational?style=flat-square) ![AppVersion: 0.0.1](https://img.shields.io/badge/AppVersion-0.0.1--informational?style=flat-square)

A Helm chart for Kubernetes

## Values

### Required BabelBot configuration

| Key                               | Type     | Default   | Description                                                                                                                                 |
| --------------------------------- | -------- | --------- | ------------------------------------------------------------------------------------------------------------------------------------------- |
| babelbot.deepL.authKey            | string   | `""`      | Your DeepL API auth key                                                                                                                     |
| babelbot.deepL.targetLanguageCode | string   | `"en-GB"` | The target language for DeepL translations (see [here](https://www.deepl.com/en/docs-api/translating-text/) for a list of supported values) |
| babelbot.telegram.allowedUsers    | string[] | `[]`      | List of Telegram user IDs allowed to use the bot                                                                                            |
| babelbot.telegram.apiKey          | string   | `""`      | Your Telegram bot's API key                                                                                                                 |

### General deployment configuration

| Key                        | Type   | Default               | Description                                                                                                                                                          |
| -------------------------- | ------ | --------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| replicaCount               | int    | `1`                   | Number of replicas of BabelBot ([currently we support only a single instance](https://github.com/RauchF/BabelBot/issues/8))                                          |
| image.repository           | string | `"babelbot/babelbot"` | Custom image                                                                                                                                                         |
| image.pullPolicy           | string | `"IfNotPresent"`      | [Image pull polic](https://kubernetes.io/docs/concepts/containers/images/#image-pull-policy)                                                                         |
| image.tag                  | string | `""`                  | Custom image tag (defaults to latest version, but not `latest`)                                                                                                      |
| imagePullSecrets           | list   | `[]`                  | Needs to be specified if you configure a custom image on [a private registry](https://kubernetes.io/docs/tasks/configure-pod-container/pull-image-private-registry/) |
| nameOverride               | string | `""`                  | Overrides the application name, but keeps the instance names (resultung in a combination of the application name and the custom name)                                |
| fullnameOverride           | string | `""`                  | Completely overrides the name                                                                                                                                        |
| serviceAccount.create      | bool   | `true`                | Creates a [service account](https://kubernetes.io/docs/tasks/configure-pod-container/configure-service-account/) for the pod                                         |
| serviceAccount.annotations | object | `{}`                  | Annotations for the service account                                                                                                                                  |
| serviceAccount.name        | string | `""`                  | Name of the service account                                                                                                                                          |
| podAnnotations             | object | `{}`                  | Pod annotations                                                                                                                                                      |
| podSecurityContext         | object | `{}`                  | Pod [security context](https://kubernetes.io/docs/tasks/configure-pod-container/security-context/)                                                                   |
| securityContext            | object | `{}`                  | Container [security context](https://kubernetes.io/docs/tasks/configure-pod-container/security-context/)                                                             |
| resources                  | object | `{}`                  | Pod [resource requests and limits](https://kubernetes.io/docs/concepts/configuration/manage-resources-containers/)                                                   |
| nodeSelector               | object | `{}`                  | [Node selector](https://kubernetes.io/docs/concepts/scheduling-eviction/assign-pod-node/#nodeselector)                                                               |
| affinity                   | object | `{}`                  | [Node affinity](https://kubernetes.io/docs/concepts/scheduling-eviction/assign-pod-node/#node-affinity)                                                              |
| tolerations                | list   | `[]`                  | [Node tolerations](https://kubernetes.io/docs/concepts/scheduling-eviction/taint-and-toleration/)                                                                    |

----------------------------------------------
Autogenerated from chart metadata using [helm-docs v1.6.0](https://github.com/norwoodj/helm-docs/releases/v1.6.0)
