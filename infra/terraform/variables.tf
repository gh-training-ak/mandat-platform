variable "environment" {
  description = "Environment short name."
  type        = string

  validation {
    condition     = contains(["dev", "test", "prod"], var.environment)
    error_message = "environment must be one of dev, test or prod."
  }
}

variable "location" {
  description = "Azure region."
  type        = string
  default     = "uksouth"
}

variable "address_space" {
  description = "VNet address space."
  type        = list(string)
  default     = ["10.42.0.0/16"]
}
