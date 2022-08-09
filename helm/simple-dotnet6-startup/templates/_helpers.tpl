{{/*
Expand the name of the chart.
*/}}
{{- define "simple-dotnet6-startup.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "simple-dotnet6-startup.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "simple-dotnet6-startup.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "simple-dotnet6-startup.labels" -}}
helm.sh/chart: {{ include "simple-dotnet6-startup.chart" . }}
{{ include "simple-dotnet6-startup.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
app: {{ include "simple-dotnet6-startup.name" . }}
app.kubernetes.io/component: {{ include "simple-dotnet6-startup.name" . }}
app.kubernetes.io/part-of: {{ include "simple-dotnet6-startup.name" . }}
app.openshift.io/runtime: dotnet
{{- end }}

{{/*
Selector labels
*/}}
{{- define "simple-dotnet6-startup.selectorLabels" -}}
app.kubernetes.io/name: {{ include "simple-dotnet6-startup.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "simple-dotnet6-startup.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "simple-dotnet6-startup.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}
